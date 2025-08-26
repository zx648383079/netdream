using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Models;
using System.Linq;

namespace NetDream.Modules.Shop
{
    public static class Extension
    {
        public static void ProvideShopRepositories(this IServiceCollection service)
        {
        }

        internal static IQueryable<CommentListItem> SelectAs(this IQueryable<CommentEntity> query)
        {
            return query.Select(i => new CommentListItem()
            {
                Id = i.Id,
                Content = i.Content,
                ItemId = i.ItemId,
                ItemType = i.ItemType,
                Rank = i.Rank,
                Title = i.Title,
                UpdatedAt = i.UpdatedAt,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt
            });
        }

        internal static void IncludeCategory(ShopContext db, ArticleListItem[] items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.ArticleCategories.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.CatId > 0 && data.TryGetValue(item.CatId, out var res))
                {
                    item.Category = res;
                }
            }
        }
    }
}
