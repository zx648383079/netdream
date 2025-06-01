using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth
{
    public static class Extension
    {
        public static void ProvideAuthRepositories(this IServiceCollection service)
        {
            service.AddScoped<AuthRepository>();
            service.AddScoped<CaptchaRepository>();
        }
    }
}
