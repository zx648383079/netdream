using Microsoft.Extensions.DependencyInjection;
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
