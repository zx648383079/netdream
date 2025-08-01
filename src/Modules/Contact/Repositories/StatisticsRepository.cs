using NetDream.Modules.Contact.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class StatisticsRepository(ContactContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.FeedbackCount = db.Feedbacks.Count();
            if (res.FeedbackCount > 0)
            {
                res.FeedbackNew = db.Feedbacks.Where(i => i.Status == 0).Count();
            }
            res.ReportCount = db.Reports.Count();
            if (res.ReportCount > 0)
            {
                res.ReportNew = db.Reports.Where(i => i.Status == 0).Count();
            }
            res.LinkCount = db.FriendLinks.Count();
            if (res.LinkCount > 0)
            {
                res.LinkToday = db.FriendLinks.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.SubscribeCount = db.Subscribes.Count();
            if (res.SubscribeCount > 0)
            {
                res.SubscribeToday = db.Subscribes.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
