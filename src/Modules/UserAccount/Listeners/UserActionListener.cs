using MediatR;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.UserAccount.Listeners
{
    public class UserActionListener(UserContext db) : INotificationHandler<UserAction>
    {
        public Task Handle(UserAction notification, CancellationToken cancellationToken)
        {
            db.ActionLogs.Add(new ActionLogEntity()
            {
                Ip = notification.Ip,
                Action = notification.Action,
                Remark = notification.Remark,
                UserId = notification.UserId,
                CreatedAt = notification.Timestamp
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
