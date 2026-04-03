using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Listeners
{
    public class DisbandTeamListener(ChatContext db) : INotificationHandler<DisbandTeam>
    {
        public Task Handle(DisbandTeam notification, CancellationToken cancellationToken)
        {
            db.Messages.Where(i => i.GroupId == notification.TeamId).ExecuteDelete();
            db.Histories.Where(i => i.ItemId == notification.TeamId && i.ItemType == 1).ExecuteDelete();
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
