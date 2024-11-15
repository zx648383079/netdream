using NetDream.Modules.Book.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Repositories
{
    public class StatisticsRepository(IDatabase db, BookRepository bookStore) : IStatisticsRepository, IUserStatistics
    {
        public IDictionary<string, int> Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today); ;
            var categoryCount = 0;
            var bookCount = db.FindCount<BookEntity>(string.Empty);
            var bookToday = bookCount > 0 ? 
                db.FindCount<BookEntity>("created_at>=@0", todayStart) : 0;

            var chapterCount = db.FindCount<ChapterEntity>(string.Empty);
            var chapterToday = chapterCount > 0 ?
                db.FindCount<ChapterEntity>("created_at>=@0", todayStart) : 0;

            var wordCount = db.FindScalar<int, ChapterEntity>("SUM(size) as total", string.Empty);
            var wordToday = wordCount > 0 ?
                db.FindScalar<int, ChapterEntity>("SUM(size) as total", "created_at>=@0", todayStart) : 0;
            
            var authorCount = db.FindCount<AuthorEntity>(string.Empty);

            var listCount = db.FindCount<ListEntity>(string.Empty);
            var listToday = listCount > 0 ?
                db.FindCount<ListEntity>("created_at>=@0", todayStart) : 0;

            var viewCount = db.FindScalar<int, BookEntity>("SUM(click_count) as total", string.Empty);
            var viewToday = viewCount > 0 ?
                bookStore.ClickLog().TodayCount(BookRepository.LOG_TYPE_BOOK, 0, BookRepository.LOG_ACTION_CLICK) : 0;

            return new Dictionary<string, int>()
            {
                {"category_count", categoryCount },
                {"author_count", authorCount },
                {"book_count", bookCount },
                {"book_today", bookToday },
                  {"chapter_count", chapterCount },
                {"chapter_today", chapterToday },
                  {"word_count", wordCount },
                {"word_today", wordToday },
                  {"list_count", listCount },
                {"list_today", listToday },
                  {"view_count", viewCount },
                {"view_today", viewToday },
            };
        }

        public IEnumerable<StatisticsItem> Subtotal(int user)
        {
            var count = db.FindCount<BookEntity>("user_id=@0", user);
            return [
                new("书籍数量", count, "本")
            ];
        }
    }
}
