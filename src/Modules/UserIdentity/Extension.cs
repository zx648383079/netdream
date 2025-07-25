using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Modules.UserIdentity
{
    public static class Extension
    {
        public static void ProvideIdentityRepositories(this IServiceCollection service)
        {
            service.AddScoped<CardRepository>();
            service.AddScoped<IdentityRepository>();
            service.AddScoped<RoleRepository>();
        }
    }
}
