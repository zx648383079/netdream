using NetDream.Modules.Note.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Note.Repositories
{
    public class StatisticsRepository(NoteContext db)
    {
        public StatisticsResult Subtotal()
        {
            var res = new StatisticsResult();
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            res.NoteCount = db.Notes.Count();
            res.NoteToday = res.NoteCount > 0 ? 
                db.Notes.Where(i => i.CreatedAt >= todayStart).Count() : 0; 
            return res;
        }

        public IEnumerable<StatisticsItem> Subtotal(int user)
        {
            return [
                new("便签数量", db.Notes.Where(i => i.UserId == user).Count(), "条")
            ];
        }
    }
}
