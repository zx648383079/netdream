using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OnlineMedia.Repositories;

namespace NetDream.Modules.OnlineMedia
{
    public static class Extension
    {
        public static void ProvideOnlineMediaRepositories(this IServiceCollection service)
        {
            service.AddScoped<TVRepository>();
        }
    }
}
