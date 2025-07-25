using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Bot.Repositories;

namespace NetDream.Modules.Bot
{
    public static class Extension
    {
        public static void ProvideBotRepositories(this IServiceCollection service)
        {
            service.AddScoped<BotRepository>();
        }
    }
}
