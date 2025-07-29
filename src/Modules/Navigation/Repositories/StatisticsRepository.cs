using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Helpers;
using System;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class StatisticsRepository(NavigationContext db)
    {
        public StatisticsResult Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var res = new StatisticsResult
            {
                SiteCount = db.Sites.Count()
            };
            if (res.SiteCount > 0)
            {
                res.SiteToday = db.Sites.Where(i => i.CreatedAt >= todayStart).Count();
            }
            res.PageCount = db.Pages.Count();
            if (res.PageCount > 0)
            {
                res.PageToday = db.Pages.Where(i => i.CreatedAt >= todayStart).Count();
            }
            return res;
        }
    }
}
