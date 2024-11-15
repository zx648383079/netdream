using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class BangRepository(IDatabase db, IClientEnvironment environment)
    {
        public List<BookModel> Recommend(int limit = 4)
        {
            return Query(sql => sql.OrderBy("recommend_count desc")
                .Limit(limit));
        }

        public List<BookModel> Hot(int limit = 10)
        {
            return Query(sql => sql.OrderBy("click_count desc")
                .Limit(limit));
        }

        public List<BookModel> NewRecommend(int limit = 12)
        {
            return Query(sql => sql.Where("size<100000")
                .OrderBy("created_at desc", "recommend_count desc")
                .Limit(limit));
        }

        public List<BookModel> WeekClick(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("click_count desc")
                .Limit(limit));
        }

        public List<BookModel> WeekRecommend(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("recommend_count desc")
                .Limit(limit));
        }

        public List<BookModel> MonthClick(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("click_count desc")
                .Limit(limit));
        }

        public List<BookModel> MonthRecommend(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("recommend_count desc")
                .Limit(limit));
        }

        public List<BookModel> Click(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("click_count desc")
                .Limit(limit));
        }

        public List<BookModel> Size(int limit = 5)
        {
            return Query(sql => sql
                .OrderBy("size desc")
                .Limit(limit));
        }

        public List<BookModel> Over(int limit = 5)
        {
            return Query(sql => sql
                .Where("over_at>0")
                .OrderBy("click_count desc")
                .Limit(limit));
        }

        private List<BookModel> Query(Action<Sql> cb)
        {
            var sql = new Sql();
            sql.Select().From<BookEntity>(db);
            if (environment.UserId == 0)
            {
                sql.Where("classify=0 and status=1");
            }
            cb.Invoke(sql);
            var items = db.Fetch<BookModel>(sql);
            WithAuthor(items);
            WithCategory(items);
            return items;
        }

        private List<CategoryEntity> GetCategories(params int[] ids)
        {
            var sql = new Sql();
            sql.Select()
                .From<CategoryEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<CategoryEntity>(sql);
        }
        private List<AuthorEntity> GetAuthors(params int[] ids)
        {
            var sql = new Sql();
            sql.Select()
                .From<AuthorEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<AuthorEntity>(sql);
        }

        private void WithCategory(IEnumerable<BookModel> items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = GetCategories(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.CatId == it.Id)
                    {
                        item.Category = it;
                        break;
                    }
                }
            }
        }

        private void WithAuthor(IEnumerable<BookModel> items)
        {
            var idItems = items.Select(item => item.AuthorId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = GetAuthors(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.AuthorId == it.Id)
                    {
                        item.Author = it;
                        break;
                    }
                }
            }
        }
    }
}
