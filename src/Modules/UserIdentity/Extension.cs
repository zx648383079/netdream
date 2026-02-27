using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Listeners;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Notifications;
using System.Linq;

namespace NetDream.Modules.UserIdentity
{
    public static class Extension
    {
        public static void ProvideIdentityRepositories(this IServiceCollection service)
        {
            service.AddScoped<IIdentityRepository, IdentityRepository>();
            service.AddScoped<CardRepository>();
            
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

        internal static IQueryable<ZoneListItem> SelectAs(this IQueryable<ZoneEntity> query)
        {
            return query.Select(i => new ZoneListItem()
            {
                Id = i.Id,
                Icon = i.Icon,
                Name = i.Name,
                Description = i.Description,
            });
        }
    }
}
