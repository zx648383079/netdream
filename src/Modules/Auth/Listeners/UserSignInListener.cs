using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class UserSignInListener(AuthContext db) : INotificationHandler<UserSignIn>
    {
        public Task Handle(UserSignIn notification, CancellationToken cancellationToken)
        {
            db.LoginLogs.Add(new LoginLogEntity
            {
                Ip = notification.Ip,
                UserId = notification.UserId,
                Mode = notification.Mode,
                Status = notification.Status ? 1 : 0,
                User = notification.Account,
                CreatedAt = notification.Timestamp
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
