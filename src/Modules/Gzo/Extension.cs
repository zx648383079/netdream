using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Gzo.Repositories;

namespace NetDream.Modules.Gzo
{
    public static class Extension
    {
        public static void ProvideGzoRepositories(this IServiceCollection service)
        {
            service.AddScoped<GzoRepository>();
        }
    }
}
