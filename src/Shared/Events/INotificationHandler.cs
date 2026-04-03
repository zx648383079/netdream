using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Shared.Events
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        public Task Handle(TNotification notification, CancellationToken token);
    }
}
