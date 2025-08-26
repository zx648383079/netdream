using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class BookRepository(BookContext db, IClientContext client)
    {
        public const int CHAPTER_TYPE_FREE_CHAPTER = 0;
        public const int CHAPTER_TYPE_VIP_CHAPTER = 1;
        public const int CHAPTER_TYPE_GROUP = 9; // 卷

        public const string DEFAULT_COVER = "/assets/images/book_default.jpg";
        public const int LOG_TYPE_BOOK = 0;
        public const int LOG_TYPE_LIST = 1;

        public const int LOG_ACTION_CLICK = 0;
        public const int LOG_ACTION_COLLECT = 3;
        public const int LOG_ACTION_AGREE = 1;
        public const int LOG_ACTION_DISAGREE = 2;

        public ActionLogProvider Log()  
        {
            return new ActionLogProvider(db, client);
        }

        public TagProvider Tag() 
        {
            return new TagProvider(db);
        }

        public DayLogProvider ClickLog() 
        {
            return new DayLogProvider(db, client);
        }

        /// <summary>
        /// 前台请求
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public IPage<BookEntity> GetList(BookQueryForm form)
        {
            return db.Books.Include(i => i.Author)
                .Include(i => i.Category)
                .Search(form.Keywords, "name")
                .When(client.UserId == 0, i => i.Classify == 0)
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Author > 0, i => i.AuthorId == form.Author)
                .Where(i => i.Status == 1)
                .When(form.Status == 1, i => i.OverAt == 0)
                .When(form.Status == 2, i => i.OverAt > 0)
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        protected IPage<BookEntity> SortByClick(IQueryable<BookEntity> query, 
            string type, PaginationForm form)
        {
            var logs = ClickLogs(type);
            var res = new Page<BookEntity>(logs.Count, form);
            if (logs.Count == 0 || res.IsEmpty)
            {
                return res;
            }
            var items = logs.Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                .ToDictionary(i => i.ItemId, i => i.Count);
            if (items.Count == 0)
            {
                return res;
            }
            var bookItems = query.Where(i => items.Keys.Contains(i.Id)).ToArray();
            foreach (var item in bookItems)
            {
                if (items.TryGetValue(item.Id, out var c))
                {
                    item.ClickCount = c;
                } else
                {
                    item.ClickCount = 0;
                }
            }
            res.Items = bookItems.OrderByDescending(i => i.ClickCount).ToArray();
            return res;
        }

        public IList<LogCount> ClickLogs(string type)
        {
            return type switch
            {
                "month" => ClickLog().SortByMonth(LOG_TYPE_BOOK, LOG_ACTION_CLICK),
                "week" => ClickLog().SortByWeek(LOG_TYPE_BOOK, LOG_ACTION_CLICK),
                "day" => ClickLog().SortByDay(LOG_TYPE_BOOK, LOG_ACTION_CLICK),
                _ => [],
            };
        }

        public IPage<BookEntity> GetManageList(BookQueryForm form)
        {
            return db.Books.Include(i => i.Author)
                .Include(i => i.Category)
                .Search(form.Keywords, "name")
                .Where(i => i.Classify == form.Classify)
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Author > 0, i => i.AuthorId == form.Author)
                .When(form.Status >= 0, i=> i.Status == form.Status)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IPage<BookEntity> GetSelfList(BookQueryForm form)
        {
            return db.Books.Include(i => i.Author)
                .Include(i => i.Category)
                .Search(form.Keywords, "name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<BookEntity> Detail(int id)
        {
            var model = db.Books.Include(i => i.Author)
                .Include(i => i.Category)
                .Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<BookEntity>.Fail("小说不存在");
            }
            if (model.Status != 1)
            {
                return OperationResult<BookEntity>.Fail("小说不存在");
            }
            // model.OnShelf = HistoryRepository.HasBook(model.Id);
            return OperationResult.Ok(model);
        }

        public IOperationResult<BookEntity> GetManage(int id)
        {
            var model = db.Books.Include(i => i.Author)
                .Include(i => i.Category)
                .Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<BookEntity>.Fail("小说不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<BookEntity> GetSelf(int id)
        {
            var model = db.Books.Where(i => i.Id == id).Single();
            if (model is null || model.UserId != client.UserId)
            {
                return OperationResult<BookEntity>.Fail("小说不存在");
            }
            //model.Chapters = (new Tree(BookChapterModel.Where("book_id", model.Id)
            //    .OrderBy("parent_id", "asc")
            //    .OrderBy("position", "asc")
            //    .OrderBy("id", "asc").Get())).MakeTree();
            return OperationResult.Ok(model);
        }

        public IOperationResult<BookEntity> Save(BookForm data, bool checkUser = false)
        {
            BookEntity? model;
            if (data.Id > 0)
            {
                model = db.Books.When(checkUser, i => i.UserId == client.UserId)
                    .Where(i => i.Id == data.Id).Single();
            } else
            {
                model = new BookEntity()
                {
                    UserId = client.UserId,
                };
            }
            if (model is null)
            {
                return OperationResult<BookEntity>.Fail("书籍不存在！");
            }
            model.Name = data.Name;
            model.Cover = data.Cover;
            model.Description = data.Description;
            model.CatId = data.CatId;
            model.AuthorId = data.AuthorId;
            model.Classify = data.Classify;
            if (IsExist(model))
            {
                return OperationResult<BookEntity>.Fail("书籍已存在！");
            }
            db.Books.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<BookEntity>.Fail("error");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<BookEntity> SaveSelf(BookForm data)
        {
            if (data.Id > 0 && !IsSelf(data.Id))
            {
                return OperationResult<BookEntity>.Fail("操作失败");
            }
            //data["user_id"] = auth().Id();
            //data["author_id"] = AuthorRepository.AuthAuthor();
            return Save(data, true);
        }

        public void Remove(int id)
        {
            db.Books.Where(i => i.Id == id).ExecuteDelete();
            var ids = db.Chapters.Where(i => i.BookId == id).Select(i => i.Id)
                .ToArray();
            if (ids.Length > 0)
            {
                db.Chapters.Where(i => i.BookId == id).ExecuteDelete();
                db.ChapterBodies.Where(i => ids.Contains(i.Id)).ExecuteDelete();
            }
        }

        public IList<ITreeItem> Chapters(int book)
        {
            var items = db.Chapters.Where(i => i.BookId == book)
                .OrderBy(i => i.ParentId)
                .OrderBy(i => i.Position)
                .OrderBy(i => i.CreatedAt).Select<ChapterEntity, ChapterTreeItem>().ToArray();
            return TreeHelper.Create(items);
        }

        public IOperationResult<ChapterModel> Chapter(int id, int book = 0)
        {
            ChapterEntity? model;
            if (id > 0)
            {
                model = db.Chapters.Where(i => i.Id == id).Single();
            } else
            {
                model = db.Chapters.Where(i => i.BookId == book)
                    .OrderBy(i => i.Position)
                    .OrderBy(i => i.CreatedAt)
                    .Single();
            }
            if (model is null)
            {
                return OperationResult.Fail<ChapterModel>("章节错误");
            }
            ClickLog().Add(LOG_TYPE_BOOK, model.BookId, 0);
            var data = model.CopyTo<ChapterModel>();
            data.Content = model.Type < 1 ? db.ChapterBodies.Where(i => i.Id == model.Id).Select(i => i.Content)
                .Single() : string.Empty;
            data.Previous = GetPrevious(model);
            data.Next = GetNext(model);
            return OperationResult.Ok(data);
        }

        private ChapterEntity? GetPrevious(ChapterEntity model)
        {
            return db.Chapters.Where(i => i.BookId == model.BookId && i.Id < model.Id)
                .OrderByDescending(i => i.Position)
                .OrderByDescending(i => i.Id)
                .Select(i => new ChapterEntity()
                {
                    Id = i.Id,
                    Title = i.Title
                }).Single();
        }

        private ChapterEntity? GetNext(ChapterEntity model)
        {
            return db.Chapters.Where(i => i.BookId == model.BookId && i.Id > model.Id)
                .OrderBy(i => i.Position)
                .OrderBy(i => i.Id)
                .Select(i => new ChapterEntity()
                {
                    Id = i.Id,
                    Title = i.Title
                }).Single();
        }

        public void RefreshBook()
        {
            DeleteNoBookChapter();
            RefreshBookSize();
        }

        protected void RefreshBookSize()
        {
            //var ids = BookModel.Pluck("id");
            //foreach (var id in ids)
            //{
            //    RefreshSize(id);
            //}
        }

        public void RefreshPosition(int book)
        {
            //data = BookChapterModel.Where("book_id", book)
            //    .OrderBy("parent_id", "asc")
            //    .OrderBy("position", "asc")
            //    .OrderBy(MigrationTable.COLUMN_CREATED_AT, "asc").Get("id", "position");
            //position = 0;
            //foreach (data as item)
            //{
            //    position++;
            //    if (intval(item["position"]) === position)
            //    {
            //        continue;
            //    }
            //    BookChapterModel.Where("id", item["id"]).Update([
            //       "position" => position
            //    ]);
            //}
        }

        public void RefreshSize(int book)
        {
            var length = db.Chapters.Where(i => i.BookId == book).Sum(i => i.Size);
            db.Books.Where(i => i.Id == book)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Size, length));
 
        }

        private void DeleteNoBookChapter()
        {
            //ids = BookChapterModel.Query().Alias("c")
            //    .Left("book b", "b.id", "=", "c.book_id")
            //.Where("b.id")
            //.Select("c.id")
            //    .Pluck();
            //if (!empty(ids))
            //{
            //    BookChapterModel.WhereIn("id", ids).Delete();
            //    BookChapterBodyModel.WhereIn("id", ids).Delete();
            //}
        }

        public bool IsSelf(int id)
        {
            return db.Books.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }

        /// <summary>
        /// 判断小说是否已存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist(BookEntity entity)
        {
            return db.Books.Where(i => i.Id != entity.Id && i.Name == entity.Name).Any();
        }

        public IOperationResult<BookEntity> OverSelf(int id)
        {
            var model = db.Books.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult.Fail<BookEntity>("操作失败");
            }
            model.OverAt = client.Now;
            db.Books.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public bool CheckOpen(int id)
        {
            return db.Books.Where(i => i.Id == id && i.Status == 1).Any();
        }

        public string[] GetHot()
        {
            return db.Books.Where(i => i.Status == 1)
                .Select(i => i.Name).Take(4).ToArray();
        }

        public string[] Suggestion(string keywords = "")
        {
            return db.Books.Search(keywords, "name").Where(i => i.Status == 1)
                .Select(i => i.Name).Take(4).ToArray();
        }

        /**
         * 整理小说id，及目录id
         */
        public void SortOut()
        {
            db.Books.RefreshPk((oldId, newId) => {
                db.Chapters.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.Sources.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.Histories.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.BuyLogs.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.Roles.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.ListItems.Where(i => i.BookId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, newId));
                db.Logs.Where(i => i.ItemId == oldId && i.ItemType == LOG_TYPE_BOOK).ExecuteUpdate(setters => setters.SetProperty(i => i.ItemId, newId));
                db.DayLogs.Where(i => i.ItemId == oldId && i.ItemType == LOG_TYPE_BOOK).ExecuteUpdate(setters => setters.SetProperty(i => i.ItemId, newId));
            });
            db.Chapters.RefreshPk((oldId, newId) => {
                db.Histories.Where(i => i.ChapterId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.ChapterId, newId));
                db.BuyLogs.Where(i => i.ChapterId == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.ChapterId, newId));
                db.ChapterBodies.Where(i => i.Id == oldId).ExecuteUpdate(setters => setters.SetProperty(i => i.Id, newId));
            });
        }
    }
}
