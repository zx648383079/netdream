using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetDream.Modules.Book.Repositories
{
    public class HistoryRepository(BookContext db, IClientContext client)
    {
        public bool HasBook(int id)
        {
            if (client.UserId == 0)
            {
                return false;
            }
            return db.Histories.Where(i => i.UserId == client.UserId && i.BookId == id).Any();
        }

        public void Log(ChapterEntity chapter)
        {
            if (client.UserId > 0)
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
            if (client.UserId == 0)
            {
                return;
            }
            var log = db.Histories.Where(i => i.UserId == client.UserId && i.BookId == book).Single();
            if (log is not null) {
                log.ChapterId = chapter;
                log.Progress = progress;
            } else
            {
                log = new HistoryEntity()
                {
                    ChapterId = chapter,
                    Progress = progress,
                    BookId = book,
                    UserId = client.UserId,

                };
            }
            db.Histories.Save(log, client.Now);
            db.SaveChanges();
        }

        public void RemoveBook(int id)
        {
            if (client.UserId == 0)
            {
                return;
            }
            db.Histories.Where(i => i.UserId == client.UserId && i.BookId == id).ExecuteDelete();
        }

        public int[] GetHistoryId()
        {
            //history = request().Cookie(BookHistoryModel.TableName());
            //return empty(history) ? [] : unserialize(history);
            return [];
        }

        public bool HasHistory(int bookId)
        {
            return db.Histories.Where(i => i.UserId == client.UserId && i.BookId == bookId).Any();
        }

        /**
         * 获取一页的章节内容
         * @return Page
         * @throws \Exception
         */
        public IPage<HistoryModel> GetHistory(int page = 1)
        {
            IPage<HistoryModel> res;
            if (client.UserId > 0)
            {
                res = db.Histories.Where(i => i.UserId == client.UserId)
                    .ToPage(page).CopyTo<HistoryEntity, HistoryModel>();
                WithBook(res.Items);
                WithChapter(res.Items);
                return res;
            }
            var items = GetHistoryId();
            if (items.Length == 0)
            {
                return new Page<HistoryModel>();
            }
            var chapterItems = db.Chapters.Where(i => items.Contains(i.Id))
                .ToPage(page);
            res = new Page<HistoryModel>(chapterItems.TotalItems, chapterItems.CurrentPage)
            {
                Items = chapterItems.Items.Select(item => new HistoryModel()
                {
                    BookId = item.BookId,
                    ChapterId = item.Id,
                    Chapter = item,
                }).ToArray()
            };
            WithBook(res.Items);
            return res;
        }

        private ChapterEntity[] GetChapters(params int[] ids)
        {
            return db.Chapters.Where(i => ids.Contains(i.Id)).ToArray();
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

        private BookEntity[] GetBooks(params int[] ids)
        {
            return db.Books.Where(i => ids.Contains(i.Id)).ToArray();
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
