using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OnlineService.Repositories;

namespace NetDream.Modules.OnlineService
{
    public static class Extension
    {
        public static void ProvideOnlineServiceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ChatRepository>();
        }
    }
}
