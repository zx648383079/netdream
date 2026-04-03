using NetDream.Modules.Article.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class StatisticsRepository(
        ArticleContext db,
        ICounter counter,
        ICommentRepository comment)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult
            {
                CategoryCount = db.Categories.Count(),
                AricleCount = db.Articles.Count()
            };
            if (res.AricleCount > 0)
            {
                res.AricleToday = db.Articles.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.ViewCount = db.Articles.Sum(i => i.ClickCount);
            if (res.ViewCount > 0)
            {
                res.ViewToday = counter.Count(ModuleTargetType.Article, DateTime.Today);
            }
            res.CommentCount = comment.Count(ModuleTargetType.Article);
            if (res.CommentCount > 0)
            {
                res.CommentToday = comment.Count(ModuleTargetType.Article, DateTime.Today);
            }
            return res;
        }
    }
}
