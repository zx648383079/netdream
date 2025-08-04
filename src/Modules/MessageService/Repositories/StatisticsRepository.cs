using NetDream.Modules.MessageService.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.MessageService.Repositories
{
    public class StatisticsRepository(MessageServiceContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.MessageCount = db.Logs.Count();
            if (res.MessageCount > 0)
            {
                res.MessageToday = db.Logs.Where(i => i.CreatedAt >= todayStart && i.Status != MessageProtocol.STATUS_SEND_FAILURE).Count();
                res.FailureToday = db.Logs.Where(i => i.CreatedAt >= todayStart && i.Status == MessageProtocol.STATUS_SEND_FAILURE).Count();
            }
            return res;
        }
    }
}
