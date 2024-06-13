using Microsoft.Extensions.DependencyInjection;

namespace NetDream.Modules.OnlineDisk
{
    public static class Extension
    {
        public static void ProvideContactRepositories(this IServiceCollection service)
        {
            // service.AddScoped<ContactRepository>();
        }
    }
}
