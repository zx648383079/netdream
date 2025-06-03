using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces.Entities;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class ArticleRepository(ShopContext db)
    {
        public ArticleListItem[] GetNotices()
        {
            var res = db.Articles.Where(i => i.CatId == 1)
                .Select(i => new ArticleListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CatId = i.CatId,
                    Description = i.Description,
                    Thumb = i.Thumb,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt,
                }).ToArray();
            Extension.IncludeCategory(db, res);
            return res;
        }

        public ITreeItem[] GetHelps()
        {
            var catId = 2;
            var data = db.ArticleCategories.Where(i => i.ParentId == catId).Select(i => new TreeListItem()
            {
                Id = i.Id,
                ParentId = i.ParentId,
                Name = i.Name,
            }).ToArray();
            foreach (var item in data)
            {
                item.Children = db.Articles.Where(i => i.CatId == item.Id)
                    .Select(i => new TreeListItem()
                    {
                        Id = i.Id,
                        ParentId = i.CatId,
                        Name = i.Title
                    }).ToArray();
            }
            var res = db.Articles.Where(i => i.CatId == catId)
                    .Select(i => new TreeListItem()
                    {
                        Id = i.Id,
                        ParentId = i.CatId,
                        Name = i.Title
                    }).ToArray();
           
            return [..res, ..data];
        }

        
    }
}
