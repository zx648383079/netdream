using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class OrderRepository(ShopContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<OrderListItem> GetList(OrderQueryForm form)
        {
            var orderIds = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(form.Conginee))
            {
                orderIds = db.OrderAddress.Where(i => i.Name == form.Conginee)
                    .When(form.Tel, i => i.Tel == form.Tel)
                    .When(form.Address, i => i.Address == form.Address)
                    .Pluck(i => i.OrderId);
            }
            if (!string.IsNullOrWhiteSpace(form.Keywords))
            {
                orderIds = db.OrderGoods.Search(form.Keywords, "name").Pluck(i => i.OrderId);
            }
            var userIds = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(form.User))
            {
                userIds = userStore.SearchUserId(form.User);
            }
            var res = db.Orders
                .When(!string.IsNullOrWhiteSpace(form.SeriesNumber), 
                i => i.SeriesNumber == form.SeriesNumber)
            .When(!string.IsNullOrWhiteSpace(form.StartAt), i => i.CreatedAt >= form.StartAtTime)
            .When(!string.IsNullOrWhiteSpace(form.EndAt), i => i.CreatedAt <= form.EndAtTime)
            .When(orderIds.Length > 0, i => orderIds.Contains(i.Id))
            .When(userIds.Length > 0, i => userIds.Contains(i.UserId))
            .When(form.Status > 0, i => i.Status == form.Status)
            .OrderByDescending(i => i.CreatedAt)
            .ToPage<OrderEntity, OrderListItem>(form);
            if (res.Items.Length == 0)
            {
                return res;
            }
            userStore.Include(res.Items);
            var idItems = res.Items.Select(item => item.Id).ToArray();
            var addressItems = db.OrderAddress.Where(i => idItems.Contains(i.OrderId)).ToDictionary(i => i.OrderId);
            var goodsItems = db.OrderGoods.Where(i => idItems.Contains(i.OrderId)).ToArray();
            foreach (var item in res.Items)
            {
                if (addressItems.TryGetValue(item.Id, out var address))
                {
                    item.Address = address;
                }
                item.Goods = goodsItems.Where(i => i.OrderId == item.Id).ToArray();
            }
            return res;
        }
        
        public IOperationResult<OrderEntity> Get(int id, bool full = false)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<OrderEntity>("订单不存在");
            }
            if (!full)
            {
                return OperationResult.Ok(model);
            }
            var res = model.CopyTo<OrderModel>();
            res.StatusLabel = OrderLabelItem.Format(model.Status);
            res.GoodsList = db.OrderGoods.Where(i => i.OrderId == model.Id).ToArray();
            res.Address = db.OrderAddress.Where(i => i.OrderId == model.Id).FirstOrDefault();
            res.User = userStore.Get(model.UserId);
            res.Delivery = db.Deliveries.Where(i => i.OrderId == model.Id).FirstOrDefault();
            return OperationResult.Ok<OrderEntity>(res);
        }

        public IOperationResult OperateShipping(int id, OrderShippingForm data)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("订单不存在");
            }
            if (!string.IsNullOrEmpty(data.ShippingId))
            {
                model.ShippingId = data.ShippingId;
                model.ShippingName = db.Shipping.Where(i => i.Code == data.ShippingId).Value(i => i.Name);
            }
            var delivery = new DeliveryEntity()
            {
                UserId = model.UserId,
                OrderId = model.Id,
                Status = 0,
                ShippingId = model.ShippingId,
                ShippingName = model.ShippingName,
                ShippingFee = model.ShippingFee,
                LogisticsNumber = data.LogisticsNumber,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            var address = db.OrderAddress.Where(i => i.OrderId == model.Id).SingleOrDefault();
            if (address is not null)
            {
                delivery.Name = address.Name;
                delivery.RegionId = address.RegionId;
                delivery.RegionName = address.RegionName;
                delivery.Tel = address.Tel;
                delivery.Address = address.Address;
                delivery.BestTime = address.BestTime;
            }
            db.Deliveries.Add(delivery);
            db.SaveChanges();
            var goodsItems = db.OrderGoods.Where(i => i.OrderId == model.Id).ToArray();
            foreach (var goods in goodsItems)
            {
                db.DeliveryGoods.Add(new DeliveryGoodsEntity()
                {
                    DeliveryId = delivery.Id,
                    OrderGoodsId = goods.Id,
                    GoodsId = goods.GoodsId,
                    Name = goods.Name,
                    Thumb = goods.Thumb,
                    SeriesNumber = goods.SeriesNumber,
                    Amount = goods.Amount,
                    ProductId = goods.ProductId,
                    TypeRemark = goods.TypeRemark,
                });
            }
            ChangeStatus(model, OrderStatus.STATUS_SHIPPED, "订单发货");
            return OperationResult.Ok(model);
        }
        public IOperationResult OperatePay(int id, OrderOperateForm data)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("订单不存在");
            }
            ChangeStatus(model, OrderStatus.STATUS_PAID_UN_SHIP, data.Remark);
            return OperationResult.Ok(model);
        }
        public IOperationResult OperateCancel(int id, OrderOperateForm data)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("订单不存在");
            }
            ChangeStatus(model, OrderStatus.STATUS_CANCEL, data.Remark);
            return OperationResult.Ok(model);
        }
        public IOperationResult OperateRefund(int id, OrderRefundForm data)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("订单不存在");
            }
            // TODO
            return OperationResult.Ok(model);
        }
        public IOperationResult OperateFee(int id, OrderFeeForm data)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("订单不存在");
            }
            OperateFee(model, data);
            return OperationResult.Ok(model);
        }
        public void OperateFee(OrderEntity order, OrderFeeForm data)
        {
            decimal total = 0;
            if (data.PayFee is not null)
            {
                var diff = order.PayFee - (decimal)data.PayFee;
                order.PayFee = (decimal)data.PayFee;
                order.OrderAmount -= diff;
                total -= diff;
            }
            if (data.ShippingFee is not null)
            {
                var diff = order.ShippingFee - (decimal)data.ShippingFee;
                order.ShippingFee = (decimal)data.ShippingFee;
                order.OrderAmount -= diff;
                total -= diff;
            }
            if (order.OrderAmount < 0)
            {
                order.OrderAmount = 0;
            }
            db.Orders.Save(order, client.Now);
            db.OrderLogs.Add(new OrderLogEntity()
            {
                OrderId = order.Id,
                UserId = client.UserId,
                Status = order.Status,
                Remark = $"调整了费用[{total}]",
                CreatedAt = client.Now
            });
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

        public OrderLabelItem[] GetSubtotal()
        {
            var res = db.Orders.GroupBy(i => i.Status)
                .Select(i => new OrderLabelItem(i.Key, i.Count())).ToList();
            res.Add(new("uncomment", "未评价")
            {
                Count = db.OrderGoods.Where(i => i.UserId == client.UserId && i.Status == OrderStatus.STATUS_RECEIVED)
                .Count()
            });
            res.Add(new("refunding", "退款中")
            {
                Count = db.OrderRefunds.Where(i => i.UserId == client.UserId && i.Status == RefundStatus.STATUS_IN_REVIEW)
                .Count()
            });
            return [..res];
        }

        public OrderLabelItem[] CheckNew()
        {
            return [
                new("paid_un_ship", "待发货") 
                {
                    Status = OrderStatus.STATUS_PAID_UN_SHIP,
                    Count = db.Orders.Where(i => i.Status == OrderStatus.STATUS_PAID_UN_SHIP).Count()
                }
            ];
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Orders.Where(i => i.Id == id).SingleOrDefault();
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
            return OperationResult.Ok();
        }
        internal static void Include(ShopContext db, IWithOrderModel[] items)
        {
            var idItems = items.Select(item => item.OrderId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Orders.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.OrderId > 0 && data.TryGetValue(item.OrderId, out var res))
                {
                    item.Order = res;
                }
            }
        }
    }
}