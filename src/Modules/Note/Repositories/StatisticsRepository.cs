using Modules.Note.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Note.Repositories
{
    public class StatisticsRepository(IDatabase db) : IStatisticsRepository, IUserStatistics
    {
        public IDictionary<string, int> Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            var noteCount = db.FindCount<NoteEntity>(string.Empty);
            var noteToday = noteCount > 0 ? 
                db.FindCount<NoteEntity>("created_at>=@0", todayStart) : 0; 
            return new Dictionary<string, int>()
            {
                {"note_count", noteCount},
                { "note_today", noteToday}
            };
        }

        public IEnumerable<StatisticsItem> Subtotal(int user)
        {
            return [
                new("便签数量", db.FindCount<NoteEntity>("user_id=@0", user), "条")
            ];
        }
    }
}
