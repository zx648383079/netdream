using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class RegionRepository(ShopContext db)
    {
        public RegionEntity[] GetList(QueryForm form, int parent = 0)
        {
            return db.Regions.Where(i => i.ParentId == parent)
                .Search(form.Keywords, "name")
                .ToArray();
        }

        public RegionEntity[] GetPath(int id)
        {
            var path = new List<RegionEntity>();
            var exit = new HashSet<int>();
            var child = id;
            while (child > 0)
            {
                if (exit.Contains(child))
                {
                    // logger().Error(sprintf("[%s] 地址有误，请及时修复", implode(",", exit)));
                    return [];
                }
                var model = db.Regions.Where(i => i.Id == child).SingleOrDefault();
                if (model is null)
                {
                    return [];
                }
                path.Add(model);
                exit.Add(child);
                child = model.ParentId;
            }
            path.Reverse();
            return [.. path];
        }

        public int[] GetPathId(int id)
        {
            return GetPath(id).Select(i => i.Id).ToArray();
        }

        public IPage<RegionEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Regions.Search(form.Keywords, "name")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id)).ToPage(form);
        }
    }
}