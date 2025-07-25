using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OnlineDisk.Repositories;

namespace NetDream.Modules.OnlineDisk
{
    public static class Extension
    {
        public static void ProvideContactRepositories(this IServiceCollection service)
        {
            service.AddScoped<DiskRepository>();
        }
    }
}
