using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class BookRepository(IDatabase db, IClientContext environment)
    {
        const string BASE_KEY = "book";
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
            return new ActionLogProvider(db, BASE_KEY, environment);
        }

        public TagProvider Tag() 
        {
            return new TagProvider(db, BASE_KEY);
        }

        public DayLogProvider ClickLog() 
        {
            return new DayLogProvider(db, BASE_KEY, environment);
        }

        /// <summary>
        /// 前台请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="keywords"></param>
        /// <param name=""></param>
        /// <param name="top"></param>
        /// <param name="status"></param>
        /// <param name="author"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        public Page<BookModel> GetList(int[] id, int category = 0, 
            string keywords = "", 
            bool top = false, 
            int status = 0, int author = 0, 
            int page = 1, int perPage = 20)
        {
            var sql = new Sql();
            sql.Select().From<BookModel>(db);
            if (environment.UserId == 0)
            {
                sql.Where("classify=0");
            }
            if (category > 0)
            {
                sql.Where("cat_id=@0", category);
            }
            SearchHelper.Where(sql, "name", keywords);
            if (author > 0)
            {
                sql.Where("author_id=@0", author);
            }
            sql.Where("status=1");
            if (status == 1)
            {
                sql.Where("over_at=0");
            } else if (status == 2) 
            {
                sql.Where("over_at>0");
            }
            var items = db.Page<BookModel>(page, 20, sql.OrderBy("id desc"));
            // TODO category author
            return items;
        }

        protected object SortByClick(Sql query, string type, int page = 1, int per_page = 20)
        {
            var logs = ClickLogs(type);
            return logs;
            //pager = new Page(count(logs), per_page, page);
            //if (empty(logs) || pager.GetTotal() < pager.GetStart())
            //{
            //    return pager.SetPage([]);
            //}
            //logs = array_splice(logs, pager.GetStart(), pager.GetPageSize());
            //if (empty(logs))
            //{
            //    return pager.SetPage([]);
            //}
            //logs = array_column(logs, "count", "item_id");
            //book_list = query.WhereIn("id", array_keys(logs)).Get();
            //foreach (book_list as item)
            //{
            //    item.ClickCount = logs[item.Id];
            //}
            //usort(book_list, (BookModel a, BookModel b) {
            //    if (a.ClickCount > b.ClickCount)
            //    {
            //        return 1;
            //    }
            //    if (a.ClickCount == b.ClickCount)
            //    {
            //        return 0;
            //    }
            //    return -1;
            //});
            //return pager.SetPage(book_list);
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

        public Page<BookModel> GetManageList(string keywords = "", 
            int category = 0, int author = 0, int classify = 0, 
            int status = -1,
            int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<BookModel>(db)
                .Where("classify=@0", classify);
            if (category > 0)
            {
                sql.Where("cat_id=@0", category);
            }
            SearchHelper.Where(sql, "name", keywords);
            if (author > 0)
            {
                sql.Where("author_id=@0", author);
            }
            if (status >= 0)
            {
                sql.Where("status=@0", status);
            }
            var items = db.Page<BookModel>(page, 20, sql.OrderBy("id desc"));
            // TODO category author
            return items;
        }

        public Page<BookModel> GetSelfList(string keywords = "", int category = 0, int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<BookModel>(db)
                .Where("user_id=@0", environment.UserId);
            if (category> 0)
            {
                sql.Where("cat_id=@0", category);
            }
            SearchHelper.Where(sql, "name", keywords);
            var items = db.Page<BookModel>(page, 20, sql.OrderBy("id desc"));
            // TODO category author
            return items;
        }

        public BookModel Detail(int id)
        {
            var model = db.SingleById<BookModel>(id);
            if (model is null)
            {
                throw new Exception("小说不存在");
            }
            if (model.Status != 1)
            {
                throw new Exception("小说不存在");
            }
            model.Category = db.SingleById<CategoryEntity>(model.CatId);
            model.Author = db.SingleById<AuthorEntity>(model.AuthorId);
            // model.OnShelf = HistoryRepository.HasBook(model.Id);
            return model;
        }

        public BookModel GetManage(int id)
        {
            var model = db.SingleById<BookModel>(id);
            if (model is null)
            {
                throw new Exception("小说不存在");
            }
            model.Category = db.SingleById<CategoryEntity>(model.CatId);
            model.Author = db.SingleById<AuthorEntity>(model.AuthorId);
            return model;
        }

        public BookModel GetSelf(int id)
        {
            var model = db.SingleById<BookModel>(id);
            if (model is null || model.UserId != environment.UserId)
            {
                throw new Exception("小说不存在");
            }
            //model.Chapters = (new Tree(BookChapterModel.Where("book_id", model.Id)
            //    .OrderBy("parent_id", "asc")
            //    .OrderBy("position", "asc")
            //    .OrderBy("id", "asc").Get())).MakeTree();
            return model;
        }

        public BookEntity Save(BookForm data)
        {
            var model = new BookModel();
            // TODO
            if (IsExist(model))
            {
                throw new Exception("书籍已存在！");
            }
            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            return model;
        }

        public BookEntity SaveSelf(BookForm data)
        {
            if (data.Id > 0 && !IsSelf(data.Id))
            {
                throw new Exception("操作失败");
            }
            //data["user_id"] = auth().Id();
            //data["author_id"] = AuthorRepository.AuthAuthor();
            return Save(data);
        }

        public void Remove(int id)
        {
            db.DeleteById<BookEntity>(id);
            var sql = new Sql();
            sql.Select("id").From<ChapterEntity>(db).Where("book_id=", id);
            var ids = db.Pluck<int>(sql);
            if (ids.Count > 0)
            {
                db.DeleteWhere<ChapterEntity>("book_id=@0", id);
                db.DeleteWhere<ChapterBodyEntity>($"id in ({string.Join(',', ids)})");
            }
        }

        public object Chapters(int book)
        {
            //return (new Tree(BookChapterModel.Where("book_id", book)
            //    .OrderBy("parent_id", "asc")
            //    .OrderBy("position", "asc")
            //    .OrderBy(MigrationTable.COLUMN_CREATED_AT, "asc").Get())).MakeTree();
            return db.Fetch<ChapterEntity>("WHERE book_id=@0", book);
        }

        public ChapterModel Chapter(int id, int book = 0)
        {

            //chapter = id > 0 ?
            //    BookChapterModel.Find(id) : BookChapterModel.Where("book_id", book)
            //        .OrderBy("position", "asc")
            //        .OrderBy(MigrationTable.COLUMN_CREATED_AT, "asc")
            //        .First();
            //if (empty(chapter))
            //{
            //    throw new \Exception("id 错误！");
            //}
            //self.ClickLog().Add(self.LOG_TYPE_BOOK, chapter.BookId, 0);
            //data = chapter.ToArray();
            //data["content"] = chapter.Type < 1 ? chapter.Body->content : string.Empty;
            //data["previous"] = chapter.Previous;
            //data["next"] = chapter.Next;
            //return data;
            return db.SingleById<ChapterModel>(id);
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
            var length = db.FindScalar<int, ChapterEntity>("SUM(size) as total", "book_id=@0", book);
            db.UpdateWhere<BookEntity>("size=@0", "id=@1", length, book);
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
            return db.FindCount<BookEntity>("id=@0 and user_id=@1", id,
                environment.UserId) > 0;
        }

        /// <summary>
        /// 判断小说是否已存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist(BookEntity entity)
        {
            return db.FindCount<BookEntity>("id<>@0 and name=@1", entity.Id, entity.Name) > 0;
        }

        public BookEntity OverSelf(int id)
        {
            var model = db.First<BookEntity>("WHERE id=@0 and user_id=@1", id,
                environment.UserId);

            if (model is null)
            {
                throw new Exception("操作失败");
            }
            model.OverAt = environment.Now;
            db.UpdateWhere<BookEntity>("over_at=@0", "id=@1", model.OverAt, model.Id);
            return model;
        }

        public bool CheckOpen(int id)
        {
            var isOpen = db.FindCount<BookEntity>("id=@0 and status=1", id) > 0;
            if (!isOpen)
            {
                throw new Exception("书籍不存在");
            }
            return true;
        }

        public string[] GetHot()
        {
            var sql = new Sql();
            sql.Select("name").From<BookEntity>(db).Where("status=1").Limit(4);
            return db.Pluck<string>(sql).ToArray();
        }

        public string[] Suggestion(string keywords = "")
        {
            var sql = new Sql();
            sql.Select("name").From<BookEntity>(db).Where("status=1");
            SearchHelper.Where(sql, "name", keywords);
            return db.Pluck<string>(sql.Limit(4)).ToArray();
        }

        /**
         * 整理小说id，及目录id
         */
        public void SortOut()
        {
            db.RefreshPk<BookEntity>((oldId, newId) => {
                db.UpdateWhere<ChapterEntity>("book_id=@0", "book_id=@1", newId, oldId);
                db.UpdateWhere<SourceEntity>("book_id=@0", "book_id=@1", newId, oldId);
                db.UpdateWhere<HistoryEntity>("book_id=@0", "book_id=@1", newId, oldId);
                db.UpdateWhere<BuyLogEntity>("book_id=@0", "book_id=@1", newId, oldId);
                db.UpdateWhere<RoleEntity>("book_id=@0", "book_id=@1", newId, oldId);
                db.UpdateWhere<ListItemEntity>("book_id=@0", "book_id=@1", newId, oldId);
                Log().Update("item_id=@0", "item_id=@1 and item_type=@2", newId, oldId, LOG_TYPE_BOOK);
                ClickLog().Update("item_id=@0", "item_id=@1 and item_type=@2", newId, oldId, LOG_TYPE_BOOK);
            });
            db.RefreshPk<ChapterEntity>((oldId, newId) => {
                db.UpdateWhere<HistoryEntity>("chapter_id=@0", "chapter_id=@1", newId, oldId);
                db.UpdateWhere<BuyLogEntity>("chapter_id=@0", "chapter_id=@1", newId, oldId);
                db.UpdateWhere<ChapterBodyEntity>("id=@0", "id=@1", newId, oldId);

            });
        }
    }
}
