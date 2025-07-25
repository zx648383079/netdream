using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Finance.Repositories;

namespace NetDream.Modules.Finance
{
    public static class Extension
    {
        public static void ProvideFinanceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ProductRepository>();
        }
    }
}
