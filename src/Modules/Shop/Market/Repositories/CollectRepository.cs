using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CollectRepository(ShopContext db, IClientContext client)
    {
        public IPage<CollectListItem> GetList(QueryForm form)
        {
            var res = db.Collects.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage<CollectEntity, CollectListItem>(form);
            GoodsRepository.Include(db, res.Items);
            return res;
        }

        public void Add(int goods)
        {
            var log = db.Collects.Where(i => i.GoodsId == goods && i.UserId == client.UserId)
                .SingleOrDefault();
            log ??= new CollectEntity()
                {
                    GoodsId = goods,
                    UserId = client.UserId
                };
            log.CreatedAt = client.Now;
            db.Collects.Save(log);
            db.SaveChanges();
        }

        public void Remove(int[] goods)
        {
            db.Collects.Where(i => i.UserId == client.UserId && goods.Contains(i.GoodsId))
            .ExecuteDelete();
        }

        public bool Toggle(int goods)
        {
            var log = db.Collects.Where(i => i.GoodsId == goods && i.UserId == client.UserId)
                .SingleOrDefault();
            if (log == null)
            {
                db.Collects.Add(new CollectEntity()
                {
                    UserId = client.UserId,
                    GoodsId = goods,
                    CreatedAt = client.Now
                });
            } else
            {
                db.Collects.Remove(log);
            }
            return log is null;
        }
    }
}
