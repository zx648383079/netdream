using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Auth.Listeners;
using NetDream.Modules.Auth.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Modules.Auth
{
    public static class Extension
    {
        public static void ProvideAuthRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, SystemUserRepository>();
            service.AddScoped<AuthRepository>();
            service.AddScoped<UserRepository>();
            service.AddScoped<ISystemBulletin, BulletinRepository>();
            service.AddScoped<CaptchaRepository>();

            service.AddTransient<INotificationHandler<CancelAccount>, CancelAccountListener>();
            service.AddTransient<INotificationHandler<ManageAction>, ManageActionListener>();

            service.AddTransient<IRequestHandler<UserStatistics, IEnumerable<StatisticsItem>>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatistics>, UserOpenStatisticsHandler>();
        }
    }
}
