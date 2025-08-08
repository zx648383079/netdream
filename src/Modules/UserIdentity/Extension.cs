using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Listeners;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Notifications;
using System.Linq;

namespace NetDream.Modules.UserIdentity
{
    public static class Extension
    {
        public static void ProvideIdentityRepositories(this IServiceCollection service)
        {
            service.AddScoped<CardRepository>();
            service.AddScoped<IdentityRepository>();
            service.AddScoped<RoleRepository>();

            service.AddTransient<INotificationHandler<UserProfileCardRequest>, UserProfileCardHandler>();
        }

        internal static IQueryable<EquityCardListItem> SelectAs(this IQueryable<EquityCardEntity> query)
        {
            return query.Select(i => new EquityCardListItem()
            {
                Id = i.Id,
                Icon = i.Icon,
                Name = i.Name,
                Status = i.Status,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
