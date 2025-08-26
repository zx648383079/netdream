using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class BangRepository(BookContext db, IClientContext client)
    {
        public BookEntity[] Recommend(int limit = 4)
        {
            return Query(sql => sql.OrderByDescending(i => i.RecommendCount)
                .Take(limit));
        }

        public BookEntity[] Hot(int limit = 10)
        {
            return Query(sql => sql.OrderByDescending(i => i.ClickCount)
                .Take(limit));
        }

        public BookEntity[] NewRecommend(int limit = 12)
        {
            return Query(sql => sql.Where(i => i.Size < 100000)
                .OrderByDescending(i => i.CreatedAt)
                .OrderByDescending(i => i.RecommendCount)
                .Take(limit));
        }

        public BookEntity[] WeekClick(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.ClickCount)
                .Take(limit));
        }

        public BookEntity[] WeekRecommend(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.RecommendCount)
                .Take(limit));
        }

        public BookEntity[] MonthClick(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.ClickCount)
                .Take(limit));
        }

        public BookEntity[] MonthRecommend(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.RecommendCount)
                .Take(limit));
        }

        public BookEntity[] Click(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.ClickCount)
                .Take(limit));
        }

        public BookEntity[] Size(int limit = 5)
        {
            return Query(sql => sql
                .OrderByDescending(i => i.Size)
                .Take(limit));
        }

        public BookEntity[] Over(int limit = 5)
        {
            return Query(sql => sql
                .Where(i => i.OverAt > 0)
                .OrderByDescending(i => i.ClickCount)
                .Take(limit));
        }

        private BookEntity[] Query(Func<IQueryable<BookEntity>, IQueryable<BookEntity>> cb)
        {
            var query = db.Books
                .Include(i => i.Author)
                .Include(i => i.Category).When(client.UserId == 0, i =>i.Classify == 0 && i.Status == 1);
            return cb.Invoke(query)
                .ToArray();
        }

    }
}
