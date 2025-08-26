using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Modules.Book.Repositories;
using System.Linq;

namespace NetDream.Modules.Book
{
    public static class Extension
    {
        public static void ProvideBookRepositories(this IServiceCollection service)
        {
            service.AddScoped<CategoryRepository>();
        }

        internal static IQueryable<HistoryListItem> SelectAs(this IQueryable<HistoryEntity> query)
        {
            return query.Select(i => new HistoryListItem()
            {
                Id = i.Id,
                BookId = i.BookId,
                ChapterId = i.ChapterId,
                SourceId = i.SourceId,
                UserId = i.UserId,
                Progress = i.Progress,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<CategoryListItem> SelectAs(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new CategoryListItem()
            {
                Id = i.Id,
                Name = i.Name,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
