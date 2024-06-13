using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Contact.Repositories;

namespace NetDream.Modules.Contact
{
    public static class Extension
    {
        public static void ProvideContactRepositories(this IServiceCollection service)
        {
            service.AddScoped<ContactRepository>();
        }
    }
}
