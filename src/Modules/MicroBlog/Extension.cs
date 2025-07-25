using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.MicroBlog.Repositories;

namespace NetDream.Modules.MicroBlog
{
    public static class Extension
    {
        public static void ProvideMicroRepositories(this IServiceCollection service)
        {
            service.AddScoped<MicroRepository>();
        }
    }
}
