using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.ResourceStore.Repositories;

namespace NetDream.Modules.ResourceStore
{
    public static class Extension
    {
        public static void ProvideResourceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ResourceRepository>();
        }
    }
}
