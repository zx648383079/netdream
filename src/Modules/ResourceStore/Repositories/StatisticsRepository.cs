using NetDream.Modules.ResourceStore.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.ResourceStore.Repositories
{
    public class StatisticsRepository(ResourceContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.ResourceCount = db.Resources.Count();
            if (res.ResourceCount > 0)
            {
                res.ResourceToday = db.Resources.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.DownloadCount = db.Resources.Sum(i => i.DownloadCount);
            res.CommentCount = db.Resources.Sum(i => i.CommentCount);
            if (res.CommentCount > 0)
            {
                res.CommentCount = db.Comments.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
