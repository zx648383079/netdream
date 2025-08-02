using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class StatisticsRepository(BlogContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.TermCount = db.Categories.Count();
            res.BlogCount = db.Blogs.Count();
            if (res.BlogCount > 0)
            {
                res.BlogToday = db.Blogs.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.ViewCount = db.Blogs.Sum(i => i.ClickCount);
            if (res.ViewCount > 0)
            {
                var today = DateTime.Today.ToString("yyyy-MM-dd");
                res.ViewToday = db.DayLogs.Where(i => i.HappenDay == today).Sum(i => i.HappenCount);
            }
            res.CommentCount = db.Comments.Count();
            if (res.CommentCount > 0)
            {
                res.CommentToday = db.Comments.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
