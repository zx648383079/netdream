using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Counter.Events;
using NetDream.Modules.Counter.Listeners;

namespace NetDream.Modules.Counter
{
    public static class Extension
    {
        public static void ProvideCounterRepositories(this IServiceCollection service)
        {
            service.AddTransient<INotificationHandler<JumpOutLog>, JumpOutListener>();
            service.AddTransient<INotificationHandler<VisitLog>, VisitListener>();
            service.AddTransient<INotificationHandler<CounterStateLog>, StateListener>();
        }
    }
}
