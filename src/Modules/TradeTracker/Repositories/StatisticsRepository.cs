using NetDream.Modules.TradeTracker.Models;

namespace NetDream.Modules.TradeTracker.Repositories
{
    public class StatisticsRepository(TrackerContext db)
    {
        public StatisticsResult Subtotal()
        {
            return new StatisticsResult();
        }
    }
}
