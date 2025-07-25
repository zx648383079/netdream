using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.UserProfile.Repositories;

namespace NetDream.Modules.UserProfile
{
    public static class Extension
    {
        public static void ProvideProfileRepositories(this IServiceCollection service)
        {
            service.AddScoped<AddressRepository>();
            service.AddScoped<BankCardRepository>();
            service.AddScoped<CertificationRepository>();
            service.AddScoped<RegionRepository>();
        }
    }
}
