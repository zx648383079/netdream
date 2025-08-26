using NetDream.Modules.AdSense.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.AdSense.Repositories
{
    public class StatisticsRepository(AdSenseContext db)
    {

        public StatisticsResult Subtotal()
        {
            var res = new StatisticsResult();
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var adCount = db.Ads.Count();
            res.AdCount = adCount;
            res.AdToday = adCount > 0 ? 
                db.Ads.Where(i => i.CreatedAt >= todayStart).Count()
                : 0
            ;
            res.PositionCount = db.Positions.Count();
            return res;
        }
    }
}
