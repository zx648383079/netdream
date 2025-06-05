using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class OrderRepository(ShopContext db, 
        IClientContext client,
        IGlobeOption option,
        ISystemBulletin bulletin)
    {
        public int OrderExpireTime => option.TryGet<int>("shop_order_expire", out var val) ? val : 3600;



        /**
         * @param int|int[] status
         * @param string keywords
         * @return Page<Order>
         * @throws Exception
         */
        public IPage<OrderListItem> GetList(QueryForm form, int status = 0)
        {
            var res = db.Orders
                .Where(i => i.UserId == client.UserId)
                .When(status > 0, i => i.Status == status)
                .Search(form.Keywords, "series_number")
                .OrderByDescending(i => i.SeriesNumber)
                .ToPage<OrderEntity, OrderListItem>(form);
            var idItems = res.Items.Select(item => item.Id).ToArray();
            var goodsItems = db.OrderGoods.Where(i => idItems.Contains(i.OrderId)).ToArray();
            foreach (var item in res.Items)
            {
                item.Goods = goodsItems.Where(i => i.OrderId == item.Id).ToArray();
            }
            return res;
        }

        public IOperationResult<OrderModel> Get(int id)
        {
            var model = db.Orders.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<OrderModel>.Fail("订单不存在");
            }
            var res = model.CopyTo<OrderModel>();
            res.Address = FormatAddress(db.OrderAddress.Where(i => i.OrderId == model.Id)
                .FirstOrDefault());
            if (model.Status == OrderStatus.STATUS_UN_PAY)
            {
                res.ExpiredAt = model.CreatedAt + OrderExpireTime;
            }
            return OperationResult.Ok(res);
        }

        public OrderAddressEntity? FormatAddress(OrderAddressEntity? model)
        {
            if (model is null)
            {
                return null;
            }
            var res = model.CopyTo<OrderAddressEntity>();
            res.Name = StrHelper.HideName(res.Name);
            res.Tel = StrHelper.HideTel(res.Tel);
            return res;
        }

        /**
         * 签收
         * @param id
         * @return OrderModel
         * @throws Exception
         */
        public IOperationResult<OrderEntity> Receive(int id)
        {
            var model = db.Orders.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            if (model.Status != OrderStatus.STATUS_SHIPPED)
            {
                return OperationResult<OrderEntity>.Fail("订单签收失败");
            }
            ChangeStatus(model, OrderStatus.STATUS_RECEIVED, string.Empty);
            return OperationResult.Ok(model);
        }

        public IOperationResult<OrderEntity> Cancel(int id)
        {
            var model = db.Orders.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            var oldStatus = model.Status;
            if (oldStatus is not OrderStatus.STATUS_UN_PAY and not OrderStatus.STATUS_PAID_UN_SHIP)
            {
                return OperationResult<OrderEntity>.Fail("订单无法取消");
            }
            //        if (oldStatus == OrderStatus.STATUS_PAID_UN_SHIP) {
            //            return OperationResult<OrderEntity>.Fail("请联系商家进行退款");
            //        }
            ChangeStatus(model, OrderStatus.STATUS_CANCEL, string.Empty);
            if (oldStatus == OrderStatus.STATUS_PAID_UN_SHIP)
            {
                var log = new PaymentRepository().GetPayedLog(model);
                if (log is null)
                {
                    return OperationResult<OrderEntity>.Fail("未遭到您的支付记录，请联系商家");
                }
                bulletin.SendAdministrator($"订单【{model.SeriesNumber}】申请退款",
                    $"订单{model.Id}【{model.SeriesNumber}】的支付流水号【{log.Id}】第三方流水号【{log.TradeNo}】,[马上查看]", 66, [
                            bulletin.Ruler.FormatLink("[马上查看]", "b/order/" + model.Id)
                    ]);
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Repurchase(int id)
        {
            var model = db.Orders.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("订单不存在");
            }
            var goods_list = db.OrderGoods
                .Where("order_id", model.Id).ToArray();
            var cartStore = new CartRepository(db);
            return cartStore.AddGoods(goods_list);
        }

        public OrderSubtotal GetSubtotal()
        {
            if (client.UserId == 0)
            {
                return new();
            }
            var data = db.Orders.Where(i => i.UserId == client.UserId)
                .GroupBy(i => i.Status)
                .Select(i => new KeyValuePair<byte, int>(i.Key, i.Count())).ToDictionary();
            var count = 0;
            var res = new OrderSubtotal()
            {
                UnPay = data.TryGetValue(OrderStatus.STATUS_UN_PAY, out count) ? count : 0,
                Shipped = data.TryGetValue(OrderStatus.STATUS_SHIPPED, out count) ? count : 0,
                Finish = data.TryGetValue(OrderStatus.STATUS_FINISH, out count) ? count : 0,
                Cancel = data.TryGetValue(OrderStatus.STATUS_CANCEL, out count) ? count : 0,
                Invalid = data.TryGetValue(OrderStatus.STATUS_INVALID, out count) ? count : 0,
                PaidUnShip = data.TryGetValue(OrderStatus.STATUS_PAID_UN_SHIP, out count) ? count : 0,
                Received = data.TryGetValue(OrderStatus.STATUS_RECEIVED, out count) ? count : 0,
                Uncomment = db.OrderGoods.Where(i => i.UserId == client.UserId && i.Status == OrderStatus.STATUS_RECEIVED).Count(),
                Refunding = db.OrderRefunds.Where(i => i.UserId == client.UserId && i.Status == RefundStatus.STATUS_IN_REVIEW).Count()
            };
            return res;
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Orders.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("订单不存在");
            }
            if (model.Status > OrderStatus.STATUS_UN_PAY)
            {
                return OperationResult.Fail("不能删除此订单");
            }
            db.Orders.Remove(model);
            db.OrderAddress.Where(i => i.OrderId == model.Id).ExecuteDelete();
            db.OrderGoods.Where(i => i.OrderId == model.Id).ExecuteDelete();
            db.OrderActivities.Where(i => i.OrderId == model.Id).ExecuteDelete();
            db.OrderCoupons.Where(i => i.OrderId == model.Id).ExecuteDelete();
            db.SaveChanges();
        }

        public void ChangeStatus(OrderEntity order, byte status, string remark)
        {
            switch (status)
            {
                case OrderStatus.STATUS_PAID_UN_SHIP:
                    order.PayAt = client.Now;
                    break;
                case OrderStatus.STATUS_SHIPPED:
                    order.ShippingAt = client.Now;
                    break;
                case OrderStatus.STATUS_RECEIVED:
                    order.ReceiveAt = client.Now;
                    break;
                case OrderStatus.STATUS_FINISH:
                    order.FinishAt = client.Now;
                    break;
            }
            if (string.IsNullOrWhiteSpace(remark))
            {
                remark = status switch
                {
                    OrderStatus.STATUS_PAID_UN_SHIP => "订单支付",
                    OrderStatus.STATUS_SHIPPED => "订单发货",
                    OrderStatus.STATUS_RECEIVED => "订单签收",
                    OrderStatus.STATUS_CANCEL => "订单取消",
                    OrderStatus.STATUS_FINISH => "订单完成",
                    _ => string.Empty
                };
            }
            order.Status = status;
            db.Orders.Save(order, client.Now);
            db.OrderGoods.Where(i => i.OrderId == order.Id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, order.Status));
            db.OrderLogs.Add(new OrderLogEntity()
            {
                OrderId = order.Id,
                UserId = client.UserId,
                Status = order.Status,
                Remark = remark,
                CreatedAt = client.Now
            });
            db.SaveChanges();
        }

    }
}
