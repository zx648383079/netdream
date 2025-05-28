using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class CouponRepository(ShopContext db, 
        IClientContext client, IUserRepository userStore)
    {
        public IPage<CouponEntity> GetList(QueryForm form)
        {
            return db.Coupons.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<CouponEntity> Get(int id)
        {
            var model = db.Coupons.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<CouponEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<CouponEntity> Save(CouponForm data)
        {
            var model = data.Id > 0 ? db.Coupons.Where(i => i.Id == data.Id).SingleOrDefault()
               : new CouponEntity();
            if (model == null)
            {
                return OperationResult.Fail<CouponEntity>("数据有误");
            }
            model.Name = data.Name;
            model.StartAt = data.StartAt;
            model.EndAt = data.EndAt;
            model.SendValue = data.SendValue;
            model.Rule = data.Rule;
            model.RuleValue = data.RuleValue;
            model.EveryAmount = data.EveryAmount;
            model.Money = data.Money;
            model.MinMoney = data.MinMoney;
            db.Coupons.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Coupons.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<CouponLogListItem> CodeList(int coupon, QueryForm form)
        {
            var res = db.CouponLogs.Where(i => i.CouponId == coupon)
                .Search(form.Keywords, "serial_number")
                .OrderByDescending(i => i.Id)
                .ToPage<CouponLogEntity, CouponLogListItem>(form);
            userStore.Include(res.Items);
            return res;
        }

        public void CodeGenerate(int coupon, int amount)
        {
            var prefix = DateTime.Now.ToString("yyyyMMddHHiiss");
            for (; amount >= 0; amount--)
            {
                db.CouponLogs.Add(new CouponLogEntity()
                {
                    CouponId = coupon,
                    SerialNumber = $"{prefix}{amount:D6}"
                });
            }
            db.SaveChanges();
        }

        public void CodeAdd(int coupon, string[] items)
        {
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                db.CouponLogs.Add(new CouponLogEntity()
                {
                    CouponId = coupon,
                    SerialNumber = item
                });
            }
            db.SaveChanges();
        }

    }
}
