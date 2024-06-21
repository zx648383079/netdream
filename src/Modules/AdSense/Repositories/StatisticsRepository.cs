using Modules.AdSense.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;

namespace NetDream.Modules.AdSense.Repositories
{
    public class StatisticsRepository(IDatabase db): IStatisticsRepository
    {

        public IDictionary<string, int> Subtotal()
        {
            var data = new Dictionary<string, int>();
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var adCount = db.FindCount<int, AdEntity>(string.Empty);
            data.Add("ad_count", adCount);
            data.Add("ad_today", adCount > 0 ? 
                db.FindCount<int, AdEntity>("created_at>=@0", todayStart)
                : 0
            );
            data.Add("position_count", db.FindCount<int, PositionEntity>(string.Empty));
            return data;
        }
    }
}
