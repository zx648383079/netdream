using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Shared.Events
{
    public sealed class Mediator(IServiceProvider serviceProvider) : IEventBus
    {
        public async Task Publish<TNotification>(TNotification notification, CancellationToken token = default) where TNotification : INotification
        {
            var notifications = serviceProvider.GetRequiredService<IEnumerable<INotificationHandler<TNotification>>>();
            foreach (var item in notifications)
            {
                await item.Handle(notification, token);
            }
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken token = default)
        {
            return serviceProvider.GetRequiredService<IRequestHandler<IRequest<TResponse>, TResponse>>().Handle(request, token);
        }
    }
}
