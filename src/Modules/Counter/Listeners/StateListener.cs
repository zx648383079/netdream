using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Counter.Events;
using NetDream.Modules.Counter.Repositories;
using NetDream.Shared.Providers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Counter.Listeners
{
    public class StateListener(CounterContext db) : INotificationHandler<CounterStateLog>
    {
        public Task Handle(CounterStateLog notification, CancellationToken cancellationToken)
        {
            if (!Uri.TryCreate(notification.Url, UriKind.Absolute, out var uri))
            {
                return Task.CompletedTask;
            }
            var logId = db.Logs.Where(i => i.Ip == notification.Ip && i.Hostname == uri.Host 
                && i.Pathname == uri.AbsolutePath && i.Queries == uri.Query
                && i.UserAgent == notification.UserAgent 
                && i.SessionId == notification.SessionId
                && i.CreatedAt >= notification.Timestamp)
                .OrderByDescending(i => i.CreatedAt).Value(i => i.Id);

            if (notification.Status == StateRepository.STATUS_LOADED && logId > 0)
            {
                db.LoadTimeLogs.Add(new()
                {
                    LogId = logId,
                    LoadTime = notification.LoadTime,
                });
            }
            if (notification.Status == StateRepository.STATUS_ENTER)
            {
                var hostId = db.Hostnames.Where(i => i.Name == uri.Host).Value(i => i.Id);
                var pathId = db.Pathnames.Where(i => i.Name == uri.AbsolutePath).Value(i => i.Id);
                if (hostId <= 0)
                {
                     // TODO
                }
                
                var log = db.PageLogs.Where(i => i.HostId == hostId && i.PathId == pathId)
                    .FirstOrDefault();
                if (log is not null)
                {
                    log.VisitCount++;
                    db.PageLogs.Update(log);
                } else
                {
                    db.PageLogs.Add(new()
                    {
                        HostId = hostId,
                        PathId = pathId,
                        VisitCount = 1,
                    });
                }
                
            }
            if (notification.Status == StateRepository.STATUS_ENTER)
            {
                var log = db.VisitorLogs.Where(i => i.Ip == notification.Ip && i.UserId == notification.UserId)
                    .FirstOrDefault();
                if (log is not null)
                {
                    log.LastAt = notification.GetTimeOrNow(notification.LeaveAt);
                    db.VisitorLogs.Update(log);
                }
                else
                {
                    db.VisitorLogs.Add(new()
                    {
                        Ip = notification.Ip,
                        UserId = notification.UserId,
                        FirstAt = notification.GetTimeOrNow(notification.EnterAt),
                        LastAt = notification.GetTimeOrNow(notification.LeaveAt)
                    });
                }
            }
            
            if (notification.Status == StateRepository.STATUS_ENTER && logId > 0)
            {
                db.StayTimeLogs.Add(new()
                {
                    LogId = logId,
                    Status = notification.Status,
                    EnterAt = notification.EnterAt,
                });
            } else if(notification.Status == StateRepository.STATUE_LEAVE && logId > 0)
            {
                db.StayTimeLogs.Where(i => i.LogId == logId
                && i.LeaveAt == 0)
                    .OrderByDescending(i => i.Id)
                    .Take(1).ExecuteUpdate(setters => setters.SetProperty(i => i.Status, notification.Status)
                    .SetProperty(i => i.LeaveAt, notification.GetTimeOrNow(notification.LeaveAt)));
            } 
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
