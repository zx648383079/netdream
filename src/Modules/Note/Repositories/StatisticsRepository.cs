using Modules.Note.Entities;
using NetDream.Core.Extensions;
using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Core.Interfaces.Entities;
using NetDream.Core.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
