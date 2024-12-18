using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NPoco;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Repositories
{
    public class HistoryRepository(IDatabase db, IClientContext environment)
    {
        public bool HasBook(object id)
        {
            if (environment.UserId == 0)
            {
                return false;
            }
            return db.FindCount<HistoryEntity>("user_id=@0 and book_id=@1",
                environment.UserId, id) > 0;
        }

        public void Log(ChapterEntity chapter)
        {
            if (environment.UserId > 0)
            {
                Record(chapter.BookId, chapter.Id);
                return;
            }
            //history = request().Cookie(BookHistoryModel.TableName());
            //history = empty(history) ? [] : unserialize(history);
            //history[chapter.BookId] = chapter.Id;
            //if (count(history) > 10)
            //{
            //    history = array_splice(history, 0, 10);
            //}
            //response().Cookie(BookHistoryModel.TableName(), serialize(history), 365 * 86400);
        }

        public void Record(int book, int chapter, float progress = 0)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            if (HasHistory(book)) {
                db.UpdateWhere<HistoryEntity>("chapter_id=@0, progress=@1", "user_id=@2 and book_id=@3",
                    chapter, progress,
                    environment.UserId, book);
                return;
            }
            db.Insert(new HistoryEntity()
            {
                ChapterId = chapter,
                Progress = progress,
                BookId = book,
                UserId = environment.UserId,
                
            });
        }

        public void RemoveBook(int id)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.DeleteWhere<HistoryEntity>("user_id=@0 and book_id=@1",
                environment.UserId, id);
        }

        public int[] GetHistoryId()
        {
            //history = request().Cookie(BookHistoryModel.TableName());
            //return empty(history) ? [] : unserialize(history);
            return [];
        }

        public bool HasHistory(int bookId)
        {
            return db.FindCount<HistoryEntity>("user_id=@0 and book_id=@1",
                environment.UserId, bookId) > 0;
        }

        /**
         * 获取一页的章节内容
         * @return Page
         * @throws \Exception
         */
        public Page<HistoryModel> GetHistory(int page = 1)
        {
            Page<HistoryModel> res;
            Sql sql;
            if (environment.UserId > 0)
            {
                sql = new Sql();
                sql.Select().From<HistoryEntity>(db)
                    .Where("user_id=@0", environment.UserId);
                res = db.Page<HistoryModel>(page, 20, sql);
                WithBook(res.Items);
                WithChapter(res.Items);
                return res;
            }
            var items = GetHistoryId();
            if (items.Length == 0)
            {
                return new Page<HistoryModel>();
            }
            sql = new Sql();
            sql.Select().From<ChapterEntity>(db)
                .WhereIn("id", items);
            var chapterItems = db.Page<ChapterEntity>(page, 20, sql);
            res = new Page<HistoryModel>()
            {
                CurrentPage = chapterItems.CurrentPage,
                ItemsPerPage = chapterItems.ItemsPerPage,
                TotalPages = chapterItems.TotalPages,
                TotalItems = chapterItems.TotalItems,
                Items = []
            };
            foreach (var item in chapterItems.Items)
            {
                res.Items.Add(new()
                {
                    BookId = item.BookId,
                    ChapterId = item.Id,
                    Chapter = item,
                });
            }
            WithBook(res.Items);
            return res;
        }

        private List<ChapterEntity> GetChapters(params int[] ids)
        {
            var sql = new Sql();
            sql.Select()
                .From<ChapterEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<ChapterEntity>(sql);
        }

        private void WithChapter(IEnumerable<HistoryModel> items)
        {
            var idItems = items.Select(item => item.ChapterId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = GetChapters(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ChapterId == it.Id)
                    {
                        item.Chapter = it;
                        break;
                    }
                }
            }
        }

        private List<BookEntity> GetBooks(params int[] ids)
        {
            var sql = new Sql();
            sql.Select()
                .From<BookEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<BookEntity>(sql);
        }
        private void WithBook(IEnumerable<HistoryModel> items)
        {
            var idItems = items.Select(item => item.BookId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = GetBooks(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.BookId == it.Id)
                    {
                        item.Book = it;
                        break;
                    }
                }
            }
        }
    }
}
