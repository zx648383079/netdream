using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Events;
using NetDream.Modules.Counter.Listeners;
using NetDream.Modules.Counter.Models;
using NetDream.Modules.Counter.Repositories;
using System.Linq;

namespace NetDream.Modules.Counter
{
    public static class Extension
    {
        public static void ProvideCounterRepositories(this IServiceCollection service)
        {
            service.AddScoped<AnalysisRepository>();
            service.AddScoped<StateRepository>();
            service.AddScoped<StatisticsRepository>();
            service.AddTransient<INotificationHandler<JumpOutLog>, JumpOutListener>();
            service.AddTransient<INotificationHandler<VisitLog>, VisitListener>();
            service.AddTransient<INotificationHandler<CounterStateLog>, StateListener>();
        }

        internal static IQueryable<JumpLogModel> SelectAs(this IQueryable<JumpLogEntity> query)
        {
            return query.Select(i => new JumpLogModel()
            {
                Id = i.Id,
                Ip = i.Ip,
                SessionId = i.SessionId,
                Referrer = i.Referrer,
                CreatedAt = i.CreatedAt,
                Url = i.Url,
                UserAgent = i.UserAgent,
            });
        }
        internal static IQueryable<StayTimeModel> SelectAs(this IQueryable<StayTimeLogEntity> query)
        {
            return query.Select(i => new StayTimeModel()
            {
                Id = i.Id,
                LogId = i.LogId,
                EnterAt = i.EnterAt,
                LeaveAt = i.LeaveAt,
                Status = i.Status,
            });
        }
    }
}
