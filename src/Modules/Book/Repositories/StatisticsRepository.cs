using NetDream.Modules.Book.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class StatisticsRepository(
        BookContext db, 
        BookRepository bookStore,
        IAuthorRepository author,
        ICounter counter)
    {
        public StatisticsResult Subtotal()
        {
            var res = new StatisticsResult();
            var todayStart = TimeHelper.TimestampFrom(DateTime.Today);
            res.CategoryCount = 0;
            res.BookCount = db.Books.Count();
            res.BookToday = res.BookCount > 0 ? 
                db.Books.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            res.ChapterCount = db.Chapters.Count();
            res.ChapterToday = res.ChapterCount > 0 ?
                db.Chapters.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            res.WordCount = db.Chapters.Sum(i => i.Size);
            res.WordToday = res.WordCount > 0 ?
                db.Chapters.Where(i => i.CreatedAt >= todayStart).Sum(i => i.Size) : 0;

            res.AuthorCount = author.Count();

            res.ListCount = db.Lists.Count();
            res.ListToday = res.ListCount > 0 ?
                db.Lists.Where(i => i.CreatedAt >= todayStart).Count() : 0;

            res.ViewCount = db.Books.Sum(i => i.ClickCount);
            res.ViewToday = res.ViewCount > 0 ?
                counter.Count(ModuleTargetType.Book, DateTime.Today) : 0;

            return res;
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
