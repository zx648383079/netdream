using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class BrandRepository(ShopContext db)
    {
        public BrandEntity[] GetList()
        {
            return db.Brands.OrderBy(i => i.Id).ToArray();
        }

        public BrandListItem[] Recommend()
        {
            var items = db.Brands.Take(4)
                .OrderByDescending(i => i.Id).ToArray();
            return items.Select(i => {
                var item = new BrandListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Keywords = i.Keywords,
                    Image = i.Logo,
                    Price = db.Goods.Where(j => j.BrandId == i.Id).Min(i => i.Price)
                };
                return item;
            }).ToArray();
        }
    }
}
