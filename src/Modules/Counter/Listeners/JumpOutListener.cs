using MediatR;
using NetDream.Modules.Counter.Events;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Counter.Listeners
{
    public class JumpOutListener(CounterContext db) : INotificationHandler<JumpOutLog>
    {
        public Task Handle(JumpOutLog notification, CancellationToken cancellationToken)
        {
            db.JumpLogs.Add(new Entities.JumpLogEntity()
            {
                Ip = notification.Ip,
                Url = notification.Url,
                Referrer = notification.Referrer,
                SessionId = notification.SessionId,
                UserAgent = notification.UserAgent,
                CreatedAt = notification.Timestamp
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
