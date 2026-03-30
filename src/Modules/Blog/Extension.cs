using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Listeners;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using System.Linq;

namespace NetDream.Modules.Blog
{
    public static class Extension
    {
        public static void ProvideBlogRepositories(this IServiceCollection service)
        {
            service.AddScoped<BlogRepository>();
            service.AddScoped<MetaRepository>();
            service.AddScoped<PublishRepository>();

            service.AddTransient<INotificationHandler<UserStatisticsRequest>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatisticsRequest>, UserOpenStatisticsHandler>();
        }

        internal static IQueryable<BlogListItem> SelectAs(this IQueryable<BlogEntity> query)
        {
            return query.Select(i => new BlogListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Language = i.Language,
                Description = i.Description,
                ProgrammingLanguage = i.ProgrammingLanguage,
                Thumb = i.Thumb,
                TermId = i.TermId,
                ParentId = i.ParentId,
                OpenType = i.OpenType,
                UserId = i.UserId,
                CommentCount = i.CommentCount,
                ClickCount = i.ClickCount,
                RecommendCount = i.RecommendCount,
                CreatedAt = i.CreatedAt,
            });
        }
        internal static IQueryable<BlogListItem> SelectAsLabel(this IQueryable<BlogEntity> query)
        {
            return query.Select(i => new BlogListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<CategoryListItem> SelectAs(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new CategoryListItem()
            {
                Id = i.Id,
                Name = i.Name,
                EnName = i.EnName,
                Keywords = i.Keywords,
                ParentId = i.ParentId,
                Thumb = i.Thumb,
                Styles = i.Styles,
                Description = i.Description,
            });
        }

        internal static IQueryable<CategoryLabelItem> SelectAsLabel(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new CategoryLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
            });
        }

        
    }
}
