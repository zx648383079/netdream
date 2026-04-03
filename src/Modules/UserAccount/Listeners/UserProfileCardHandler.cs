using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.UserAccount.Listeners
{
    public class UserProfileCardHandler(UserContext db) : INotificationHandler<UserProfileCardRequest>
    {
        public Task Handle(UserProfileCardRequest notification, CancellationToken cancellationToken)
        {
            var user = db.Users.Where(i => i.Id == notification.UserId).SingleOrDefault();
            if (user is null)
            {
                notification.Result.Name = "[DELETED]";
            } else
            {
                notification.Add(user);
                notification.Result.UpdatedAt = TimeHelper.TimestampTo(user.UpdatedAt);
            }
            return Task.CompletedTask;
        }
    }
}
