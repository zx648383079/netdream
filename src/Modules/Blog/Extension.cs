using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Blog.Listeners;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Modules.Blog
{
    public static class Extension
    {
        public static void ProvideBlogRepositories(this IServiceCollection service)
        {
            service.AddScoped<BlogRepository>();
            service.AddScoped<CommentRepository>();
            service.AddScoped<CategoryRepository>();
            service.AddScoped<LogRepository>();
            service.AddScoped<MetaRepository>();
            service.AddScoped<PublishRepository>();

            service.AddTransient<IRequestHandler<UserStatistics, IEnumerable<StatisticsItem>>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatistics>, UserOpenStatisticsHandler>();
        }
    }
}
