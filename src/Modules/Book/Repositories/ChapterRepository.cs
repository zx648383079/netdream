using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Repositories
{
    public class ChapterRepository(IDatabase db, 
        BookRepository bookStore,
        IClientEnvironment environment)
    {
        public const int TYPE_FREE_CHAPTER = 0;
        public const int TYPE_VIP_CHAPTER = 1;
        public const int TYPE_GROUP = 9; // 卷
        public Page<ChapterEntity> GetList(int book, 
            string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<ChapterEntity>(db)
                .Where("book_id=@0", book);
            SearchHelper.Where(sql, "title", keywords);
            sql.OrderBy("position asc", "created_at asc");
            return db.Page<ChapterEntity>(page, 20, sql);
        }

        public ChapterModel Get(int id)
        {
            var chapter = db.SingleById<ChapterModel>(id);
            if (chapter is null)
            {
                throw new Exception("id 错误！");
            }
            if (chapter.Type == TYPE_GROUP)
            {
                return chapter;
            }
            chapter.Content = db.FindScalar<string, ChapterBodyEntity>("content", "id=@0", id);
            return chapter;
        }

        public ChapterEntity Save(ChapterForm data)
        {
            var model = data.Id > 0 ? db.SingleById<ChapterEntity>(data.Id) : new ChapterEntity();
            // TODO
            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            bookStore.RefreshSize(model.BookId);
            return model;
        }

        public void Remove(int id)
        {
            var model = db.SingleById<ChapterEntity>(id);
            if (model is null)
            {
                return;
            }
            db.DeleteById<ChapterEntity>(id);
            db.DeleteById<ChapterBodyEntity>(id);
            bookStore.RefreshSize(model.BookId);
        }

        public ChapterModel GetSelf(int id)
        {
            var chapter = db.SingleById<ChapterModel>(id);
            if (chapter is null || bookStore.IsSelf(chapter.BookId))
            {
                throw new Exception("id 错误！");
            }
            if (chapter.Type == TYPE_GROUP)
            {
                return chapter;
            }
            chapter.Content = db.FindScalar<string, ChapterBodyEntity>("content", "id=@0", id);
            return chapter;
        }

        public ChapterEntity SaveSelf(ChapterForm data)
        {
            var model = db.SingleById<ChapterEntity>(data.Id);
            if (!bookStore.IsSelf(data.BookId))
            {
                throw new Exception("操作无效");
            }
            if (model.Id == 0 && model.Position < 1)
            {
                model.Position = db.FindScalar<int, ChapterEntity>("MAX(position) as pos",
                    "book_id=@0", data.BookId) + 1;
            }


            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            bookStore.RefreshSize(model.BookId);
            return model;
        }

        public void RemoveSelf(int id)
        {
            var model = db.SingleById<ChapterEntity>(id);
            if (model is null)
            {
                return;
            }
            if (!bookStore.IsSelf(model.BookId))
            {
                throw new Exception("操作无效");
            }
            db.DeleteById<ChapterEntity>(id);
            db.DeleteById<ChapterBodyEntity>(id);
            bookStore.RefreshSize(model.BookId);
        }

        /**
         * 批量设置是否已购买
         * @param items
         * @return mixed
         */
        public void ApplyIsBought(int bookId, IEnumerable<ChapterModel> items)
        {
            var idItems = new List<int>();
            foreach (var item in items)
            {
                if (item.Type == 1)
                {
                    idItems.Add(item.Id);
                }
            }
            // TODO 获取已购买的id
            if (idItems.Count == 0)
            {
                return;
            }
            var sql = new Sql();
            sql.Select("chapter_id").From<BuyLogEntity>(db)
                .Where("book_id=@0 and user_id=@1", bookId, environment.UserId);
            var boughtItems = db.Pluck<int>(sql);
            foreach (var item in items)
            {
                item.IsBought = item.Type != 1 || boughtItems.Contains(item.Id);
            }
        }

        public bool IsBought(int bookId, int chapterId, int chapterType)
        {
            if (chapterType == TYPE_FREE_CHAPTER || chapterType == 
                TYPE_GROUP)
            {
                return true;
            }
            if (environment.UserId == 0)
            {
                return false;
            }
            return db.FindCount<BuyLogEntity>("book_id=@0 and chapter_id=@1 and user_id=@2",
                bookId, chapterId, environment.UserId) > 0;
        }

        public void Move(int id, int before = 0, int after = 0)
        {
            if (before < 1 && after < 1)
            {
                throw new Exception("请选择定位点");
            }
            if (before > 0)
            {
                MoveBefore(id, before);
                return;
            }
            MoveAfter(id, after);
        }

        public void MoveBefore(int id, int before)
        {
            var (model, beforeModel) = CheckPosition(id, before);
            if (model.Position < beforeModel.Position)
            {
                db.UpdateWhere<ChapterEntity>("position=position-1",
                    "book_id=@0 and position>@1 and position<@2",
                    model.BookId,
                    model.Position,
                    beforeModel.Position);
                db.UpdateWhere<ChapterEntity>("position=@0",
                    "id=@1",
                    beforeModel.Position - 1,
                    id);
                return;
            }
            db.UpdateWhere<ChapterEntity>("position=position+1",
                    "book_id=@0 and position<@1 and position>=@2",
                    model.BookId,
                    model.Position,
                    beforeModel.Position);
            db.UpdateWhere<ChapterEntity>("position=@0",
                "id=@1",
                beforeModel.Position,
                id);
        }

        private (ChapterEntity, ChapterEntity) CheckPosition(int id, int twoId)
        {
            if (id == twoId)
            {
                throw new Exception("章节错误");
            }
            var model = db.SingleById<ChapterEntity>(id);
            if (model is null)
            {
                throw new Exception("章节不存在");
            }
            if (model.Position < 1)
            {
                bookStore.RefreshPosition(model.BookId);
            }
            model.Position = db.FindScalar<int, ChapterEntity>("position", "id=@0", id);
            var twoModel = db.First<ChapterEntity>("WHERE id=@0 and book_id=@1", twoId,
                model.BookId);
            if (twoModel is null)
            {
                throw new Exception("章节不存在");
            }
            return (model, twoModel);
        }

        public void MoveAfter(int id, int after)
        {
            var (model, afterModel) = CheckPosition(id, after);
            if (model.Position < afterModel.Position)
            {
                db.UpdateWhere<ChapterEntity>("position=position-1",
                    "book_id=@0 and position>@1 and position<=@2",
                    model.BookId,
                    model.Position,
                    afterModel.Position);
                db.UpdateWhere<ChapterEntity>("position=@0",
                    "id=@1",
                    afterModel.Position,
                    id);
                return;
            }
            db.UpdateWhere<ChapterEntity>("position=position+1",
                    "book_id=@0 and position<@1 and position>@2",
                    model.BookId,
                    model.Position,
                    afterModel.Position);
            db.UpdateWhere<ChapterEntity>("position=@0",
                "id=@1",
                afterModel.Position + 1,
                id);
        }
    }
}
