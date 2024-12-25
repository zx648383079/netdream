using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class StatisticsRepository(BookContext db, BookRepository bookStore) : IStatisticsRepository, IUserStatistics
    {
        public IDictionary<string, int> Subtotal()
        {
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today); ;
            var categoryCount = 0;
            var bookCount = db.Books.Count();
            var bookToday = bookCount > 0 ? 
                db.Books.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            var chapterCount = db.Chapters.Count();
            var chapterToday = chapterCount > 0 ?
                db.Chapters.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            var wordCount = db.Chapters.Sum(i => i.Size);
            var wordToday = wordCount > 0 ?
                db.Chapters.Where(i => i.CreatedAt >= todayStart).Sum(i => i.Size) : 0;
            
            var authorCount = db.Authors.Count();

            var listCount = db.Lists.Count();
            var listToday = listCount > 0 ?
                db.Lists.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            var viewCount = db.Books.Sum(i => i.ClickCount);
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
            var count = db.Books.Where(i => i.UserId == user).Count();
            return [
                new("书籍数量", count, "本")
            ];
        }
    }
}
