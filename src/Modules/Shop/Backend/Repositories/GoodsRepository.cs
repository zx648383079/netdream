using NetDream.Modules.Shop.Backend.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class GoodsRepository
    {
        internal static void Include(ShopContext db, IWithGoodsModel[] items)
        {
            var idItems = items.Select(item => item.GoodsId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Goods.Where(i => idItems.Contains(i.Id))
                .Select(i => new GoodsLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Thumb = i.Thumb,
                    SeriesNumber = i.SeriesNumber,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.GoodsId > 0 && data.TryGetValue(item.GoodsId, out var res))
                {
                    item.Goods = res;
                }
            }
        }
    }
}