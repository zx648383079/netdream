using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CouponRepository(ShopContext db, IClientContext client)
    {
        public IPage<CouponEntity> GetCanReceive(QueryForm form, int category = 0)
        {
            var time = client.Now;
            return db.Coupons.Where(i => i.SendType == 0 && i.StartAt <= time && i.EndAt > time)
                .When(category > 0, i => i.Rule == CouponType.RULE_CATEGORY 
                && i.RuleValue == category.ToString())
                .ToPage(form);
        }

        public IPage<CouponEntity> GetMy(QueryForm form, int status)
        {
            var ids = db.CouponLogs.Where(i => i.UserId == client.UserId)
                .When(status < 1 || status == 2, i => i.UsedAt == 0)
                .When(status == 1, i => i.UsedAt > 0).Pluck(i => i.CouponId);
            if (ids.Length == 0)
            {
                return new Page<CouponEntity>();
            }
            var time = client.Now;
            return db.Coupons.Where(i => ids.Contains(i.Id))
                .When(status == 2, i => i.EndAt < time)
                .When(status < 1, i => i.EndAt > time).ToPage(form);
        }

        public CouponEntity[] GetMyUseGoods(object[] goods_list)
        {
            return GetUserUseGoods(client.UserId, goods_list);
        }

        public CouponEntity[] GetUserUseGoods(int userId, object[] goods_list)
        {
            if (goods_list.Length == 0)
            {
                return [];
            }
            var ids = db.CouponLogs.Where(i => i.UserId == userId
                && i.UsedAt == 0).Pluck(i => i.CouponId);
            if (ids.Length == 0)
            {
                return [];
            }
            var time = client.Now;
            var items = db.Coupons.Where(i => ids.Contains(i.Id) 
                && i.EndAt > time).ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            return items.Where(i => CanUse(i, goods_list)).ToArray();
        }

        public IOperationResult Receive(int id)
        {
            var coupon = db.Coupons.Where(i => i.Id == id && i.SendType == CouponType.SEND_RECEIVE)
                .SingleOrDefault();
            if (coupon is null)
            {
                return OperationResult.Fail("优惠券错误!");
            }
            if (!CanReceive(coupon))
            {
                return OperationResult.Fail("领取失败!");
            }
            db.CouponLogs.Save(new CouponLogEntity()
            {
                UserId = client.UserId,
                CouponId = coupon.Id,
            }, client.Now);
            db.SaveChanges();
            return OperationResult.Ok();
        }
        public bool CanReceive(CouponEntity model)
        {
            return CanReceive(model.Id);
        }
        public bool CanReceive(int id)
        {
            if (client.UserId == 0)
            {
                return false;
            }
            return !db.CouponLogs.Where(i => i.UserId == client.UserId && i.CouponId == id)
                .Any();
        }

        /**
         * @param CouponEntity item
         * @param CartModel[] goods_list
         * @return bool
         */
        public bool CanUse(CouponEntity item, object[] goods_list)
        {
            var time = client.Now;
            if (item.StartAt > time || item.EndAt < time)
            {
                return false;
            }
            if (item.Rule == CouponType.RULE_NONE)
            {
                return true;
            }
            var range = item.RuleValue.Split(',');
            foreach (var goods in goods_list)
            {
                if (CanUseCheckGoods(item.Rule, range, goods.Goods)) 
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanUse(CouponEntity item, GoodsEntity[] goods_list)
        {
            var time = client.Now;
            if (item.StartAt > time || item.EndAt < time)
            {
                return false;
            }
            if (item.Rule == CouponType.RULE_NONE)
            {
                return true;
            }
            var range = item.RuleValue.Split(',');
            foreach (var goods in goods_list)
            {
                if (CanUseCheckGoods(item.Rule, range, goods))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanUse(CouponEntity item, GoodsEntity goods)
        {
            return CanUseCheckGoods(item.Rule, item.RuleValue.Split(','), goods);
        }

        /**
         * 验证单个商品
         * @param rule
         * @param array range
         * @param GoodsModel goods
         * @return bool
         */
        private bool CanUseCheckGoods(byte rule, string[] range, GoodsEntity goods)
        {
            if (rule == CouponType.RULE_GOODS)
            {
                return range.Contains(goods.Id.ToString());
            }
            if (rule == CouponType.RULE_BRAND)
            {
                return range.Contains(goods.BrandId.ToString());
            }
            if (rule != CouponType.RULE_CATEGORY)
            {
                return true;
            }
            var catMaps = new CategoryRepository(db).Path(goods.CatId);
            return catMaps.Where(i => range.Contains(i.ToString())).Any();
        }

        /**
         * 获取当前商品能领取使用的优惠券
         * @param GoodsModel goods
         * @return array
         */
        public CouponEntity[] GoodsCanReceive(GoodsEntity goods)
        {
            var time = client.Now;
            var items = db.Coupons.Where(i => i.SendType == 0
                && i.StartAt <= time && i.EndAt > time)
                .ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            return items.Where(i => CanUse(i, goods)).ToArray();
        }

        public IOperationResult<CouponLogEntity> Exchange(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return OperationResult<CouponLogEntity>.Fail("优惠码错误");
            }
            var log = db.CouponLogs.Where(i => i.SerialNumber == code
                && i.UserId == 0).OrderByDescending(i => i.Id).FirstOrDefault();
            if (log is null)
            {
                return OperationResult<CouponLogEntity>.Fail("优惠码错误");
            }
            log.UserId = client.UserId;
            db.CouponLogs.Save(log, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(log);
        }

        public IOperationResult<CouponEntity> CheckCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return OperationResult<CouponEntity>.Fail("优惠码错误");
            }
            var log = db.CouponLogs.Where(i => i.SerialNumber == code
                && i.UserId == 0).OrderByDescending(i => i.Id).FirstOrDefault();
            if (log is null)
            {
                return OperationResult<CouponEntity>.Fail("优惠码错误");
            }
            var time = client.Now;
            var coupon = db.Coupons.Where(i => i.Id == log.CouponId
                && i.StartAt <= time && i.EndAt > time).SingleOrDefault();
            if (coupon is null)
            {
                return OperationResult<CouponEntity>.Fail("优惠码错误");
            }
            return OperationResult.Ok(coupon);
        }
    }
}
