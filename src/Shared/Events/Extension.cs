using Microsoft.Extensions.DependencyInjection;

namespace NetDream.Shared.Events
{
    public static class Extension
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, Mediator>();
            return services;
        }
    }
}
