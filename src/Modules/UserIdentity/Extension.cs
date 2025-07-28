using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserIdentity.Listeners;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Notifications;

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
    }
}
