using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Plan.Repositories;

namespace NetDream.Modules.Plan
{
    public static class Extension
    {
        public static void ProvidePlanRepositories(this IServiceCollection service)
        {
            service.AddScoped<PlanRepository>();
        }
    }
}
