using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Counter.Events;
using NetDream.Modules.Counter.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Counter.Listeners
{
    public class StateListener(CounterContext db) : INotificationHandler<CounterStateLog>
    {
        public Task Handle(CounterStateLog notification, CancellationToken cancellationToken)
        {
            //db.ClickLogs.Add(new()
            //{

            //});
            if (notification.Status == StateRepository.STATUS_LOADED)
            {
                db.LoadTimeLogs.Add(new()
                {
                    Url = notification.Url,
                    SessionId = notification.SessionId,
                    Ip = notification.Ip,
                    UserAgent = notification.UserAgent,
                    LoadTime = notification.LoadTime,
                });
            }
            if (notification.Status == StateRepository.STATUS_ENTER)
            {
                var log = db.PageLogs.Where(i => i.Url == notification.Url)
                    .FirstOrDefault();
                if (log is not null)
                {
                    log.VisitCount++;
                    db.PageLogs.Update(log);
                } else
                {
                    db.PageLogs.Add(new()
                    {
                        Url = notification.Url,
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
            
            if (notification.Status == StateRepository.STATUS_ENTER)
            {
                db.StayTimeLogs.Add(new()
                {
                    Url = notification.Url,
                    Ip = notification.Ip,
                    UserAgent = notification.UserAgent,
                    SessionId = notification.SessionId,
                    Status = notification.Status,
                    EnterAt = notification.EnterAt,
                });
            } else if(notification.Status == StateRepository.STATUE_LEAVE)
            {
                db.StayTimeLogs.Where(i => i.Url == notification.Url
                && i.LeaveAt == 0 && i.SessionId == notification.SessionId)
                    .OrderByDescending(i => i.Id)
                    .Take(1).ExecuteUpdate(setters => setters.SetProperty(i => i.Status, notification.Status)
                    .SetProperty(i => i.LeaveAt, notification.GetTimeOrNow(notification.LeaveAt)));
            } 
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
