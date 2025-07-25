using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.CMS.Repositories;

namespace NetDream.Modules.CMS
{
    public static class Extension
    {
        public static void ProvideMicroRepositories(this IServiceCollection service)
        {
            service.AddScoped<CMSRepository>();
        }
    }
}
