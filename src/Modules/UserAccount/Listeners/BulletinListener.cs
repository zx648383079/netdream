using MediatR;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Notifications;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NetDream.Modules.UserAccount.Listeners
{
    public class BulletinListener(UserContext db) : INotificationHandler<BulletinRequest>
    {
        public async Task Handle(BulletinRequest notification, CancellationToken cancellationToken)
        {
            var users = notification.Users.Select(i => i == BulletinRequest.AdministratorPlaceholder ? 1 : i)
                .Where(i => i > 0).ToArray();
            var bulletin = new BulletinEntity()
            {
                Title = HttpUtility.HtmlEncode(notification.Title),
                Content = HttpUtility.HtmlEncode(notification.Content),
                Type = (byte)notification.Type,
                UserId = notification.Sender,
                ExtraRule = notification.ExtraRule?.Length > 0 ? JsonSerializer.Serialize(notification.ExtraRule)
                : string.Empty,
                CreatedAt = notification.SendAt,
                Items = users.Select(i => {
                    return new BulletinUserEntity()
                    {
                        UserId = i,
                        CreatedAt = notification.SendAt,
                    };
                }).ToArray()
            };
            db.Bulletins.Add(bulletin);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
