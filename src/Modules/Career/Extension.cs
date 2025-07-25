using Microsoft.Extensions.DependencyInjection;

namespace NetDream.Modules.Career
{
    public static class Extension
    {
        public static void ProvideCareerRepositories(this IServiceCollection service)
        {
            // service.AddScoped<PageRepository>();
        }
    }
}
