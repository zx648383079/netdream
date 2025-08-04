using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Listeners;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Notifications;
using System.Linq;

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
            service.AddTransient<INotificationHandler<BulletinRequest>, BulletinListener>();

            service.AddTransient<INotificationHandler<UserStatisticsRequest>, UserStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserOpenStatisticsRequest>, UserOpenStatisticsHandler>();
            service.AddTransient<INotificationHandler<UserProfileCardRequest>, UserProfileCardHandler>();
        }

        internal static IQueryable<AccountLogListItem> SelectAs(this IQueryable<AccountLogEntity> query)
        {
            return query.Select(i => new AccountLogListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                ItemId = i.Id,
                Money = i.Money,
                Remark = i.Remark,
                Status = i.Status,
                Type = i.Type,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
