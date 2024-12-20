using MediatR;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Events;
using NPoco;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class ManageActionListener(IDatabase db) : INotificationHandler<ManageAction>
    {
        public Task Handle(ManageAction notification, CancellationToken cancellationToken)
        {
            db.Insert(new AdminLogEntity()
            {
                Ip = notification.Ip,
                Action = notification.Action,
                Remark = notification.Remark,
                ItemType = notification.ItemType,
                ItemId = notification.ItemId,
                CreatedAt = notification.CreateAt
            });
            return Task.CompletedTask;
        }
    }
}
