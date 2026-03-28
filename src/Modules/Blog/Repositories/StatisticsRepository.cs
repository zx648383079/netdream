using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class StatisticsRepository(
        BlogContext db,
        ICounter counter,
        ICommentRepository commentStore)
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
                res.ViewToday = counter.Count(ModuleTargetType.Blog, DateTime.Today);
            }
            res.CommentCount = commentStore.Count(ModuleTargetType.Blog);
            if (res.CommentCount > 0)
            {
                res.CommentToday = commentStore.Count(ModuleTargetType.Blog, DateTime.Today);
            }
            return res;
        }
    }
}
