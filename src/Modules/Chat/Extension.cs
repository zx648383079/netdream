using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Chat.Repositories;

namespace NetDream.Modules.Chat
{
    public static class Extension
    {
        public static void ProvideChatRepositories(this IServiceCollection service)
        {
            service.AddScoped<ChatRepository>();
        }
    }
}
