using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Listeners;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using System.Linq;

namespace NetDream.Modules.Article
{
    public static class Extension
    {
        public static void ProvideArticleRepositories(this IServiceCollection service)
        {

            service.AddTransient<INotificationHandler<UserStatisticsRequest>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatisticsRequest>, UserOpenStatisticsHandler>();
        }


        internal static IQueryable<AuthorLabelItem> SelectAsLabel(this IQueryable<AuthorEntity> query)
        {
            return query.Select(i => new AuthorLabelItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Name = i.Name,
                Avatar = i.Avatar,
                Description = i.Description,
            });
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
