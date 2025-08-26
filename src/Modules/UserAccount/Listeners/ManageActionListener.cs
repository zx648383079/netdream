using MediatR;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.UserAccount.Listeners
{
    public class ManageActionListener(UserContext db) : INotificationHandler<ManageAction>
    {
        public Task Handle(ManageAction notification, CancellationToken cancellationToken)
        {
            db.AdminLogs.Add(new AdminLogEntity()
            {
                Ip = notification.Ip,
                Action = notification.Action,
                Remark = notification.Remark,
                ItemType = (int)notification.ItemType,
                ItemId = notification.ItemId,
                CreatedAt = notification.Timestamp
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
