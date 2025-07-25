using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Navigation.Repositories;

namespace NetDream.Modules.Navigation
{
    public static class Extension
    {
        public static void ProvideNavigationRepositories(this IServiceCollection service)
        {
            service.AddScoped<SiteRepository>();
        }
    }
}
