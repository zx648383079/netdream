using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Catering.Repositories;

namespace NetDream.Modules.Catering
{
    public static class Extension
    {
        public static void ProvideCateringRepositories(this IServiceCollection service)
        {
            service.AddScoped<CategoryRepository>();
        }
    }
}
