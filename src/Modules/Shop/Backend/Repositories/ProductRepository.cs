using NetDream.Modules.Shop.Backend.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    internal class ProductRepository
    {
        internal static void Include(ShopContext db, IWithProductModel[] items)
        {
            var idItems = items.Select(item => item.ProductId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Products.Where(i => idItems.Contains(i.Id))
                .Select(i => new ProductLabelItem()
                {
                    Id = i.Id,
                    Attributes = i.Attributes,
                    SeriesNumber = i.SeriesNumber,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.ProductId > 0 && data.TryGetValue(item.ProductId, out var res))
                {
                    item.Product = res;
                }
            }
        }
    }
}