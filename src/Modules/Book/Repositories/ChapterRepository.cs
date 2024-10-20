using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            // BookRepository.RefreshSize(model.BookId);
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
            // BookRepository.RefreshSize(model.BookId);
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
        public void ApplyIsBought(int bookId, Page<ChapterModel> items)
        {
            //idItems = [];
            //foreach (items as item)
            //{
            //    if (item["type"] == 1)
            //    {
            //        idItems[] = item["id"];
            //    }
            //}
            //// TODO 获取已购买的id
            //boughtItems = empty(idItems) ? [] : BookBuyLogModel.Where("book_id", bookId)
            //    .Where("user_id", auth().Id()).Pluck("chapter_id");
            //foreach (items as item)
            //{
            //    item["is_bought"] = item["type"] != 1 || in_array(item["id"], boughtItems);
            //}
            //return items;
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
            //list(model, beforeModel) = self.CheckPosition(id, before);
            //if (model.Position < beforeModel.Position)
            //{
            //    BookChapterModel.Query().Where("book_id", model.BookId)
            //        .Where("position", ">", model.Position)
            //        .Where("position", "<", beforeModel.Position)
            //        .UpdateDecrement("position");
            //    BookChapterModel.Where("id", id).Update([
            //        "position" => beforeModel.Position - 1
            //    ]);
            //    return;
            //}
            //BookChapterModel.Query().Where("book_id", model.BookId)
            //    .Where("position", "<", model.Position)
            //    .Where("position", ">=", beforeModel.Position)
            //    .UpdateIncrement("position");
            //BookChapterModel.Where("id", id).Update([
            //    "position" => beforeModel.Position
            //]);
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
            //list(model, afterModel) = CheckPosition(id, after);
            //if (model.Position < afterModel.Position)
            //{
            //    BookChapterModel.Query().Where("book_id", model.BookId)
            //        .Where("position", ">", model.Position)
            //        .Where("position", "<=", afterModel.Position)
            //        .UpdateDecrement("position");
            //    BookChapterModel.Where("id", id).Update([
            //        "position" => afterModel.Position
            //    ]);
            //    return;
            //}
            //BookChapterModel.Query().Where("book_id", model.BookId)
            //    .Where("position", "<", model.Position)
            //    .Where("position", ">", afterModel.Position)
            //    .UpdateIncrement("position");
            //BookChapterModel.Where("id", id).Update([
            //    "position" => afterModel.Position + 1
            //]);
        }
    }
}
