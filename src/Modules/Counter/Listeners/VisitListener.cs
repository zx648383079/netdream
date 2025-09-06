using MediatR;
using NetDream.Modules.Counter.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using UAParser;

namespace NetDream.Modules.Counter.Listeners
{
    public class VisitListener(CounterContext db) : INotificationHandler<VisitLog>
    {
        public Task Handle(VisitLog notification, CancellationToken cancellationToken)
        {
            // var uaParser = Parser.GetDefault();
            // var clientInfo = uaParser.Parse(notification.UserAgent);
            Uri.TryCreate(notification.Url, UriKind.Absolute, out var uri);
            Uri.TryCreate(notification.Referrer, UriKind.Absolute, out var refer);

            db.Logs.Add(new Entities.LogEntity()
            {
                //Os = clientInfo.OS.Family,
                //OsVersion = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}",
                //Browser = clientInfo.UA.Family,
                //BrowserVersion = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}",
                Hostname = uri.Host,
                Pathname = uri.AbsolutePath,
                Queries = uri.Query,
                ReferrerHostname = refer.Host,
                ReferrerPathname = refer.PathAndQuery,
                Ip = notification.Ip,
                UserAgent = notification.UserAgent,
                UserId = notification.UserId,
                SessionId = notification.SessionId,
                CreatedAt = notification.Timestamp,
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
