using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.AdSense.Repositories
{
    public class StatisticsRepository(AdSenseContext db): IStatisticsRepository
    {

        public IDictionary<string, int> Subtotal()
        {
            var data = new Dictionary<string, int>();
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var adCount = db.Ads.Count();
            data.Add("ad_count", adCount);
            data.Add("ad_today", adCount > 0 ? 
                db.Ads.Where(i => i.CreatedAt >= todayStart).Count()
                : 0
            );
            data.Add("position_count", db.Positions.Count());
            return data;
        }
    }
}
