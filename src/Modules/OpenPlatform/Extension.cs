using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OpenPlatform.Repositories;

namespace NetDream.Modules.OpenPlatform
{
    public static class Extension
    {
        public static void ProvideOpenRepositories(this IServiceCollection service)
        {
            service.AddScoped<OpenRepository>();
        }
    }
}
