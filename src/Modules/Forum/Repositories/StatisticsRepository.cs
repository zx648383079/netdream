using NetDream.Modules.Forum.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.Forum.Repositories
{
    public class StatisticsRepository(ForumContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult
            {
                ForumCount = db.Forums.Count(),
                ThreadCount = db.Threads.Count(),
                ThreadToday = db.Threads.Where(i => i.CreatedAt >= todayStart).Count(),
                PostCount = db.ThreadPosts.Count(),
                PostToday = db.ThreadPosts.Where(i => i.CreatedAt >= todayStart).Count(),
                ViewCount = db.Threads.Sum(i => i.ViewCount),
                ViewToday = db.Threads.Where(i => i.CreatedAt >= todayStart).Sum(i => i.ViewCount)
            };
            return res;
        }
    }
}
