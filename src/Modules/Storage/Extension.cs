using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Storage.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Storage
{
    public static class Extension
    {
        public static void ProvideStorageRepositories(this IServiceCollection service)
        {
            service.AddScoped<IStorageRepository, FileRepository>();
        }
    }
}
