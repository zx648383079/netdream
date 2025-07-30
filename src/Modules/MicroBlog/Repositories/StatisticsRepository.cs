using NetDream.Modules.MicroBlog.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class StatisticsRepository(MicroBlogContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult();
            res.PostCount = db.Blogs.Count();
            if (res.PostCount > 0)
            {
                res.PostToday = db.Blogs.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.TopicCount = db.Topics.Count();
            if (res.TopicCount > 0)
            {
                res.TopicToday = db.Topics.Where(i => i.CreatedAt >= todayStart).Count();
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
