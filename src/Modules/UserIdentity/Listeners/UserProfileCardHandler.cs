using MediatR;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.UserIdentity.Listeners
{
    public class UserProfileCardHandler(IdentityContext db) : INotificationHandler<UserProfileCardRequest>
    {
        public Task Handle(UserProfileCardRequest notification, CancellationToken cancellationToken)
        {
            if (notification.IsInclude("card_items"))
            {
                notification.Add(CardRepository.GetUserCard(db, notification.UserId));
            }
            return Task.CompletedTask;
        }
    }
}
