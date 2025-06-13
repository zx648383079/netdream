using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Forms;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Modules.Trade.Models;
using NetDream.Modules.UserProfile;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CashierRepository(
        ShopContext db, 
        IClientContext client,
        IGlobeOption option,
        ProfileContext profileDB)
    {
        /// <summary>
        /// 货到付款的 code
        /// </summary>
        public const string COD_CODE = "cod";

        public StockStore Store(int status = 0)
        {
            var store = new StockStore(db, option)
            {
                OrderStatus = status
            };
            return store;
        }
        public AddressEntity? FormatAddress(int user, AddressForm address)
        {
            if (address.Id > 0)
            {
                return FormatAddress(user, address.Id);
            }
            if (address.RegionId < 0 || string.IsNullOrWhiteSpace(address.Tel) || string.IsNullOrWhiteSpace(address.Address))
            {
                return null;
            }
            return new AddressEntity()
            {
                Name = address.Name,
                Address = address.Address,
                RegionId = address.RegionId,
                Tel = address.Tel,
            };
        }
        public AddressEntity? FormatAddress(int user, int address)
        {
            return profileDB.Address.Where(i => i.UserId == user && i.Id == address).SingleOrDefault();
        }

        public CouponLogEntity? FormatCoupon(int user, 
            int coupon, string couponCode)
        {
            if (coupon > 0)
            {
                return db.CouponLogs.Where(i => i.UserId == user
                    && i.Id == coupon && i.OrderId == 0).SingleOrDefault();
            }
            if (string.IsNullOrWhiteSpace(couponCode))
            {
                return null;
            }
            return db.CouponLogs.Where(i => (i.UserId == user || i.UserId == 0) 
                && i.SerialNumber == couponCode && i.OrderId == 0)
                .FirstOrDefault();
        }

        public IOperationResult<ShippingListItem[]> ShipList(int userId, 
            int addressId, ICartItem[] goods_list)
        {
            var address = FormatAddress(userId, addressId);
            if (address is null)
            {
                return OperationResult<ShippingListItem[]>.Fail("地址错误");
            }
            var provider = new ShippingRepository(db, profileDB);
            var data = provider.GetByAddress(address);
            if (data.Length == 0)
            {
                return OperationResult<ShippingListItem[]>.Fail("当前地址不在配送范围内");
            }
            foreach (var item in data)
            {
                item.ShippingFee = provider.GetFee(item, item.Settings, goods_list);
            }
            return OperationResult.Ok(data);
        }

        public object[] PaymentList(int shipping, ICartItem[] goods_list)
        {
            //data = PaymentModel.Query().Get();
            //return data;
            return [];
        }

        public CouponEntity[] CouponList(int userId, ICartItem[] goods_list)
        {
            return new CouponRepository(db, client).GetUserUseGoods(userId, goods_list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsItems"></param>
        /// <param name="address"></param>
        /// <param name="shippingCode"></param>
        /// <param name="paymentCode"></param>
        /// <param name="coupon"></param>
        /// <param name="couponCode"></param>
        /// <param name="isPreview">如果只验证，则配送方式和支付方式可空</param>
        /// <returns></returns>
        public IOperationResult<OrderPreview> Preview(int userId, ICartItem[] goodsItems, 
            AddressEntity address, string shippingCode, string paymentCode, int coupon = 0, 
            string couponCode = "", bool isPreview = true)
        {
            if (goodsItems.Length == 0)
            {
                return OperationResult<OrderPreview>.Fail("请选择结算的商品");
            }
            var order = new OrderPreview(goodsItems);
            if (address is null)
            {
                return OperationResult<OrderPreview>.Fail("收货地址无效或不完整");
            }
            // var address = FormatAddress(userId, address);
            order.Add(address);
            var couponModel = FormatCoupon(userId, coupon, couponCode);
            if (couponModel is not null && !new CouponRepository(db, client)
                .CanUse(couponModel, goodsItems))
            {
                return OperationResult<OrderPreview>.Fail("优惠卷不能使用在此订单");
                // TODO 减去优惠金额
            }
            if (IsPayment(paymentCode, out var paymentModel))
            {
                order.Add(paymentModel);
            } else if (!isPreview)
            {
                return OperationResult<OrderPreview>.Fail("请选择支付方式");
            }
            if (!string.IsNullOrEmpty(shippingCode))
            {
                var ship = db.Shipping.Where(i => i.Code == shippingCode)
                    .SingleOrDefault()?.CopyTo<ShippingListItem>();
                if (ship is null)
                {
                    return OperationResult<OrderPreview>.Fail("配送方式不存在");
                }
                var shipGroup = new ShippingRepository(db, null)
                    .GetGroup(ship.Id, address.RegionId);
                if (shipGroup is null)
                {
                    return OperationResult<OrderPreview>.Fail("当前地址不支持此配送方式");
                }
                order.Add(ship, shipGroup);
                order.ShippingFee = new ShippingRepository(db, profileDB).GetFee(ship, shipGroup, goodsItems);
                if (!isPreview)
                {
                    return OperationResult<OrderPreview>.Fail("请选择配送方式");
                }
                if (paymentCode == COD_CODE && !ship.CodEnabled)
                {
                    return OperationResult<OrderPreview>.Fail(
                        "当前配送方式不支持【货到付款】支付方式"
                    );
                }
            }
            else if(!isPreview) 
            {
                return OperationResult<OrderPreview>.Fail("请选择配送方式");
            }
            return OperationResult.Ok(order);
        }

        private bool IsPayment(string code, [NotNullWhen(true)] out ICodeOptionItem? model)
        {
            if (string.IsNullOrEmpty(code))
            {
                model = null;
                return false;
            }
            if (code == COD_CODE)
            {
                model = new PaymentListItem()
                {
                    Name = "货到付款",
                    Code = COD_CODE,
                };
                return true;
            }
            model = null;
            return model is not null;
        }


        /**
         * 结算
         * @param int userId
         * @param int|array address
         * @param int shipping
         * @param int payment
         * @param int coupon
         * @param string coupon_code
         * @param string cart
         * @param int type
         * @return OrderModel
         * @throws Exception
         */
        public IOperationResult<OrderEntity> Checkout(int userId,
            ICartItem[] goodsItems, AddressEntity address, 
            string shippingCode, string paymentCode, int coupon = 0, 
            string coupon_code = "")
        {
            var store = Store(StockStore.STATUS_ORDER);
            if (!store.Frozen(goodsItems))
            {
                return OperationResult<OrderEntity>.Fail("库存不足！");
            }
            var success = false;
            var res = Preview(userId, goodsItems, address, shippingCode, paymentCode, coupon, coupon_code, false);
            if (!res.Succeeded)
            {
                return OperationResult<OrderEntity>.Fail(res);
            }
            var data = res.Result;
            var model = new OrderEntity()
            {
                UserId = userId,
                Status = OrderStatus.STATUS_UN_PAY,
                SeriesNumber = client.Now.ToString(),
                PaymentId = data.Payment!.Code,
                PaymentName = data.Payment!.Name,
                ShippingId = data.Shipping!.Code,
                ShippingName = data.Shipping!.Name,
                PayFee = data.PayFee,
                ShippingFee = data.ShippingFee,
                GoodsAmount = data.GoodsAmount,
                OrderAmount = data.OrderAmount,
                Discount = data.Discount,
                UpdatedAt = client.Now,
                CreatedAt = client.Now
            };
            db.Orders.Add(model);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult<OrderEntity>.Fail("操作失败，请重试");
            }
            db.OrderAddress.Add(new OrderAddressEntity()
            {
                OrderId = model.Id,
                Name = data.Address.Name,
                Tel = data.Address.Tel,
                RegionId = data.Address.RegionId,
                RegionName = data.Address.RegionName,
                Address = data.Address.Address,
                BestTime = data.Address.BestTime,
            });
            foreach (var item in data.Goods)
            {
                db.OrderGoods.Add(new OrderGoodsEntity()
                {
                    OrderId = model.Id,
                    UserId = model.UserId,
                    GoodsId = item.GoodsId,
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Thumb = item.Thumb,
                    SeriesNumber = item.SeriesNumber,
                    Price = item.Price,
                    Amount = item.Amount,
                });
            }
            db.SaveChanges();
            store.Clear();
            
            //if (type < 1)
            //{
            //    CartRepository.Load().Remove(...goods_list);
            //}
            return OperationResult.Ok(model);
        }

        /**
         * 获取结算商品
         * @param mixed cart
         * @param int type
         * @return ICartItem[]
         * @throws \Exception
         */
        public ICartItem[] GetGoodsList(int[] cart)
        {
            var store = new CartRepository(db, client).Instance;
            return store.Where(i => cart.Contains(i.Id)).ToArray();
        }

        public ICartItem[] GetGoodsList(CartItemForm[] cart)
        {
            var data = new List<ICartItem>();
            var provider = new CartRepository(db, client);
            foreach (var item in cart)
            {
                if (item.Goods == 0 && item.GoodsId == 0)
                {
                    continue;
                }
                var goods = provider.GetGoods(item.Goods > 0 ? item.Goods : item.GoodsId);
                if (goods is null || goods.Status != GoodsStatus.STATUS_SALE)
                {
                    continue;
                }
                var properties = item.Properties ?? string.Empty;
                if (string.IsNullOrWhiteSpace(properties) && !string.IsNullOrWhiteSpace(item.AttributeId))
                {
                    properties = item.AttributeId;
                }
                data.Add(provider.FormatCartItem(goods.Id, properties, Math.Max(item.Amount, 1)));
            }
            return [..data];
        }

        public ICartItem[] GetGoodsList(string cart = "", int type = 0)
        {
            if (type < 1)
            {
                return GetGoodsList(cart.Split('-').Select(int.Parse).ToArray());
            }
            return GetGoodsList(JsonSerializer.Deserialize<CartItemForm[]>(cart));
        }

    }
}
