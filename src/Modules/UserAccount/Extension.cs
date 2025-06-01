using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserAccount.Events;
using NetDream.Modules.UserAccount.Listeners;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount
{
    public static class Extension
    {
        public static void ProvideUserRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, SystemUserRepository>();
            service.AddScoped<UserRepository>();
            service.AddScoped<ISystemBulletin, SystemBulletinRepository>();
            service.AddScoped<BulletinRepository>();

            service.AddTransient<INotificationHandler<CancelAccount>, CancelAccountListener>();
            service.AddTransient<INotificationHandler<ManageAction>, ManageActionListener>();

            service.AddTransient<INotificationHandler<UserStatistics>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatistics>, UserOpenStatisticsHandler>();
        }
    }
}
