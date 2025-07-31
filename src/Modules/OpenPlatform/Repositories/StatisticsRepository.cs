using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class StatisticsRepository(OpenContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.PlatformCount = db.Platforms.Count();
            if (res.PlatformCount > 0)
            {
                res.PlatformToday = db.Platforms.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
