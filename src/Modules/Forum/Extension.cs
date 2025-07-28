using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using System.Linq;

namespace NetDream.Modules.Forum
{
    public static class Extension
    {
        public static void ProvideForumRepositories(this IServiceCollection service)
        {
            service.AddScoped<ForumRepository>();
        }

        internal static IQueryable<ForumListItem> SelectAs(this IQueryable<ForumEntity> query)
        {
            return query.Select(i => new ForumListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Thumb = i.Thumb,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<ThreadListItem> SelectAs(this IQueryable<ThreadEntity> query)
        {
            return query.Select(i => new ThreadListItem()
            {
                Id = i.Id,
                Title = i.Title,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<PostListItem> SelectAs(this IQueryable<ThreadPostEntity> query)
        {
            return query.Select(i => new PostListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Content = i.Content,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<ThreadLogModel> SelectAs(this IQueryable<ThreadLogEntity> query)
        {
            return query.Select(i => new ThreadLogModel()
            {
                Id = i.Id,
                UserId = i.UserId,
                ItemType = i.ItemType,
                ItemId = i.ItemId,
                CreatedAt = i.CreatedAt,
                NodeIndex = i.NodeIndex,
                Action = i.Action,
            });
        }

        internal static IQueryable<ModeratorListItem> SelectAs(this IQueryable<ForumModeratorEntity> query)
        {
            return query.Select(i => new ModeratorListItem()
            {
                UserId = i.UserId,
                ForumId = i.ForumId,
                RoleId = i.RoleId,
            });
        }
    }
}
