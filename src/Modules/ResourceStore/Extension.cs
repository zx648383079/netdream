using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using System.Linq;

namespace NetDream.Modules.ResourceStore
{
    public static class Extension
    {
        public static void ProvideResourceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ResourceRepository>();
        }

        internal static IQueryable<ResourceListItem> SelectAs(this IQueryable<ResourceEntity> query)
        {
            return query.Select(i => new ResourceListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Keywords = i.Keywords,
                Thumb = i.Thumb,
                Size = i.Size,
                Score = i.Score,
                UserId = i.UserId,
                PreviewType = i.PreviewType,
                CatId = i.CatId,
                Price = i.Price,
                IsCommercial = i.IsCommercial,
                IsReprint = i.IsReprint,
                CommentCount = i.CommentCount,
                DownloadCount = i.DownloadCount,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                ViewCount = i.ViewCount,
            });
        }
    }
}
