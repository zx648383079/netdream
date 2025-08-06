using NetDream.Modules.Bot.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class StatisticsRepository(BotContext db)
    {

        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.AccountCount = db.Bots.Count();
            if (res.AccountCount > 0)
            {
                res.AccountToday = db.Bots.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.UserCount = db.Users.Where(i => i.Status == BotRepository.STATUS_SUBSCRIBED).Count();
            if (res.UserCount > 0)
            {
                res.UserToday = db.Users.Where(i => i.Status == BotRepository.STATUS_SUBSCRIBED && i.UpdatedAt >= todayStart).Count();
            }
            res.MessageCount = db.MessageHistories.Where(i => i.Type == BotRepository.TYPE_REQUEST).Count();
            if (res.MessageCount > 0)
            {
                res.MessageToday = db.MessageHistories.Where(i => i.Type == BotRepository.TYPE_REQUEST && i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
