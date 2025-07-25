using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Legwork.Repositories;

namespace NetDream.Modules.Legwork
{
    public static class Extension
    {
        public static void ProvideLegworkRepositories(this IServiceCollection service)
        {
            service.AddScoped<LegworkRepository>();
        }
    }
}
