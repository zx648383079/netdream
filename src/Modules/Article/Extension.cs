using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Listeners;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
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

        internal static IQueryable<ArticleListItem> SelectAs(this IQueryable<ArticleEntity> query)
        {
            return query.Select(i => new ArticleListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Language = i.Language,
                Description = i.Description,
                Thumb = i.Thumb,
                CatId = i.CatId,
                ParentId = i.ParentId,
                OpenType = i.OpenType,
                UserId = i.UserId,
                CommentCount = i.CommentCount,
                ClickCount = i.ClickCount,
                LikeCount = i.LikeCount,
                CreatedAt = i.CreatedAt,
            });
        }
        internal static IQueryable<ArticleListItem> SelectAsLabel(this IQueryable<ArticleEntity> query)
        {
            return query.Select(i => new ArticleListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<CategoryEntity> SelectAs(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new CategoryEntity()
            {
                Id = i.Id,
                Name = i.Name,
                EnName = i.EnName,
                ParentId = i.ParentId,
                Thumb = i.Thumb,
                Description = i.Description,
            });
        }

        internal static IQueryable<IListLabelItem> SelectAsLabel(this IQueryable<CategoryEntity> query)
        {
            return query.Select(i => new ListLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
            });
        }
    }
}
