using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Models;
using NetDream.Modules.Navigation.Repositories;
using System.Linq;

namespace NetDream.Modules.Navigation
{
    public static class Extension
    {
        public static void ProvideNavigationRepositories(this IServiceCollection service)
        {
            service.AddScoped<SiteRepository>();
        }

        internal static IQueryable<SiteListItem> SelectAs(this IQueryable<SiteEntity> query)
        {
            return query.Select(i => new SiteListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Logo = i.Logo,
                CatId = i.CatId,
            });
        }

        internal static IQueryable<CategoryTreeItem> SelectAs(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new CategoryTreeItem()
            {
                Id = i.Id,
                Name = i.Name,
                Icon = i.Icon,
                ParentId = i.ParentId,
            });
        }

        internal static IQueryable<PageListItem> SelectAs(this IQueryable<PageEntity> query)
        {
            return query.Select(i => new PageListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Thumb = i.Thumb,
                Description = i.Description,
                Link = i.Link,
                SiteId = i.SiteId,
                UserId = i.UserId,
            });
        }

        internal static IQueryable<ScoringLogListItem> SelectAs(this IQueryable<SiteScoringLogEntity> query)
        {
            return query.Select(i => new ScoringLogListItem()
            {
                Id = i.Id,
                SiteId = i.SiteId,
                UserId = i.UserId,
                Score = i.Score,
                LastScore = i.LastScore,
                ChangeReason = i.ChangeReason,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
