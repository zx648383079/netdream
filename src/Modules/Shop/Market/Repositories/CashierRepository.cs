using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Forms;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Modules.UserProfile;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CashierRepository(
        ShopContext db, 
        IClientContext client,
        IGlobeOption option,
        ProfileContext profileDB)
    {
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

        /**
         * @param int userId
         * @param array goods_list
         * @param int address
         * @param int shipping
         * @param int payment
         * @param int coupon
         * @param bool isPreview // 如果只验证，则配送方式和支付方式可空
         * @return OrderModel
         * @throws Exception
         */
        public IOperationResult Preview(int userId, ICartItem[] goods_list, 
            AddressEntity address, int shipping, int payment, int coupon = 0, 
            string coupon_code = "", bool isPreview = true)
        {
            if (goods_list.Length == 0)
            {
                return OperationResult.Fail("请选择结算的商品");
            }
            order = OrderModel.Preview(goods_list);
            if (empty(address))
            {
                return OperationResult.Fail("收货地址无效或不完整");
            }
            //address = FormatAddress(userId, address);
            if (address is null || !order.SetAddress(address))
            {
                return OperationResult.Fail("请选择收货地址");
            }
            coupon = FormatCoupon(userId, coupon, coupon_code);
            if (!empty(coupon) && !CouponRepository.CanUse(coupon.Coupon, goods_list))
            {
                return OperationResult.Fail("优惠卷不能使用在此订单");
                // TODO 减去优惠金额
            }
            if (payment > 0 && !order.SetPayment(PaymentModel.Find(payment)) && !isPreview)
            {
                return OperationResult.Fail("请选择支付方式");
            }
            if (shipping > 0)
            {
                ship = ShippingModel.Find(shipping);
                if (empty(ship))
                {
                    return OperationResult.Fail("配送方式不存在");
                }
                shipGroup = ShippingRepository.GetGroup(shipping, address.RegionId);
                if (empty(shipGroup))
                {
                    return OperationResult.Fail("当前地址不支持此配送方式");
                }
                ship.Settings = shipGroup;
                if (!order.SetShipping(ship) && !isPreview)
                {
                    return OperationResult.Fail("请选择配送方式");
                }
                if (payment > 0 && !ship.CanUsePayment(order.Payment))
                {
                    return OperationResult.Fail(
                        $"当前配送方式不支持【{payment.Name}】支付方式"
                    );
                }
            }
            else if(!isPreview) 
            {
                return OperationResult.Fail("请选择配送方式");
            }
            return order;
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
        public IOperationResult Checkout(int userId,
            ICartItem[] goods_list, object address, 
            int shipping, int payment, int coupon = 0, 
            string coupon_code = "")
        {
            var store = Store(StockStore.STATUS_ORDER);
            if (!store.Frozen(goods_list))
            {
                return OperationResult.Fail("库存不足！");
            }
            var success = false;
            var order = Preview(userId, goods_list, address, shipping, payment, coupon, coupon_code, false);
            if (order.CreateOrder(userId))
            {
                success = true;
                store.Clear();
            }
            else
            {
                order.Restore();
            }
            if (!success)
            {
                return OperationResult.Fail("操作失败，请重试");
            }
            //if (type < 1)
            //{
            //    CartRepository.Load().Remove(...goods_list);
            //}
            return order;
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
