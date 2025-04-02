using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Auth.Listeners;
using NetDream.Modules.Auth.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Auth
{
    public static class Extension
    {
        public static void ProvideAuthRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, SystemUserRepository>();
            service.AddScoped<AuthRepository>();
            service.AddScoped<UserRepository>();
            service.AddScoped<ISystemBulletin, SystemBulletinRepository>();
            service.AddScoped<BulletinRepository>();
            service.AddScoped<CaptchaRepository>();

            service.AddTransient<INotificationHandler<CancelAccount>, CancelAccountListener>();
            service.AddTransient<INotificationHandler<ManageAction>, ManageActionListener>();

            service.AddTransient<INotificationHandler<UserStatistics>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatistics>, UserOpenStatisticsHandler>();
        }
    }
}
