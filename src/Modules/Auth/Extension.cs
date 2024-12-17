using Microsoft.Extensions.DependencyInjection;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth
{
    public static class Extension
    {
        public static void ProvideAuthRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<AuthRepository>();
            service.AddScoped<ISystemBulletin, BulletinRepository>();
            service.AddScoped<CaptchaRepository>();
        }
    }
}
