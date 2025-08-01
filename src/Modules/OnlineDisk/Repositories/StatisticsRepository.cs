using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class StatisticsRepository(OnlineDiskContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.ServerCount = db.Servers.Count();
            if (res.ServerCount > 0)
            {
                res.OnlineCount = db.Servers.Where(i => i.Status == 1).Count();
            }
            res.FileCount = db.Files.Count();
            if (res.FileCount > 0)
            {
                res.FileToday = db.Files.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
