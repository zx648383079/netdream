using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.MicroBlog.Entities;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Modules.MicroBlog.Repositories;
using System.Linq;

namespace NetDream.Modules.MicroBlog
{
    public static class Extension
    {
        public static void ProvideMicroRepositories(this IServiceCollection service)
        {
            service.AddScoped<MicroRepository>();
            service.AddScoped<StatisticsRepository>();
            service.AddScoped<TopicRepository>();
            service.AddScoped<UserRepository>();
        }

        internal static IQueryable<TopicListItem> SelectAs(this IQueryable<TopicEntity> query)
        {
            return query.Select(i => new TopicListItem()
            {
                Id = i.Id,
                Name = i.Name,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<PostListItem> SelectAs(this IQueryable<BlogEntity> query)
        {
            return query.Select(i => new PostListItem()
            {
                Id = i.Id,
                Content = i.Content,
                ExtraRule = i.ExtraRule,
                CommentCount  = i.CommentCount,
                RecommendCount = i.RecommendCount,
                CollectCount = i.CollectCount,
                ForwardId = i.ForwardId,
                ForwardCount = i.ForwardCount,
                UserId = i.UserId,
                Source = i.Source,
                OpenType = i.OpenType,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
