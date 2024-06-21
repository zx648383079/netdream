using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.MessageService.Repositories;

namespace NetDream.Modules.MessageService
{
    public static class Extension
    {
        public static void ProvideMessageRepositories(this IServiceCollection service)
        {
            service.AddScoped<MessageProtocol>();
            service.AddScoped<MessageRepository>();
        }
    }
}
