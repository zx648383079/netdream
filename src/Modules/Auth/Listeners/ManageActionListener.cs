using MediatR;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Events;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class ManageActionListener(AuthContext db) : INotificationHandler<ManageAction>
    {
        public Task Handle(ManageAction notification, CancellationToken cancellationToken)
        {
            db.AdminLogs.Add(new AdminLogEntity()
            {
                Ip = notification.Ip,
                Action = notification.Action,
                Remark = notification.Remark,
                ItemType = notification.ItemType,
                ItemId = notification.ItemId,
                CreatedAt = notification.CreateAt
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
