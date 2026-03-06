using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Notifications;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Listeners
{
    public class CancelAccountListener(ChatContext db) : INotificationHandler<CancelAccount>
    {
        public Task Handle(CancelAccount notification, CancellationToken cancellationToken)
        {
            db.Histories.Where(i => i.UserId == notification.User.Id).ExecuteDelete();
            db.Messages.Where(i => i.UserId == notification.User.Id).ExecuteDelete();
            db.Histories.Where(i => i.ItemId == notification.User.Id && i.ItemType == 0).ExecuteDelete();
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
