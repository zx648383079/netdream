using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.UserAccount.Listeners
{
    public class CancelAccountListener : INotificationHandler<CancelAccount>
    {
        public Task Handle(CancelAccount notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
