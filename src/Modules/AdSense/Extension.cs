using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.AdSense.Repositories;

namespace NetDream.Modules.AdSense
{
    public static class Extension
    {
        public static void ProvideAdRepositories(this IServiceCollection service)
        {
            service.AddScoped<AdRepository>();
        }
    }
}
