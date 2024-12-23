using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Note.Repositories
{
    public class StatisticsRepository(NoteContext db) : IStatisticsRepository, IUserStatistics
    {
        public IDictionary<string, int> Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var noteCount = db.Notes.Count();
            var noteToday = noteCount > 0 ? 
                db.Notes.Where(i => i.CreatedAt >= todayStart).Count() : 0; 
            return new Dictionary<string, int>()
            {
                {"note_count", noteCount},
                { "note_today", noteToday}
            };
        }

        public IEnumerable<StatisticsItem> Subtotal(int user)
        {
            return [
                new("便签数量", db.Notes.Where(i => i.UserId == user).Count(), "条")
            ];
        }
    }
}
