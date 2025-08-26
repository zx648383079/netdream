using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class ChapterRepository(BookContext db, 
        BookRepository bookStore,
        IClientContext client)
    {
        public const int TYPE_FREE_CHAPTER = 0;
        public const int TYPE_VIP_CHAPTER = 1;
        public const int TYPE_GROUP = 9; // 卷
        public IPage<ChapterEntity> GetList(ChapterQueryForm form)
        {
            return db.Chapters.Where(i => i.BookId == form.Book)
                .Search(form.Keywords, "title")
                .OrderBy(i => i.Position)
                .ThenBy(i => i.CreatedAt).ToPage(form);
        }

        public IOperationResult<ChapterModel> Get(int id, bool checkUser = false)
        {
            var chapter = db.Chapters.Where(i => i.Id == id).Single()?.CopyTo<ChapterModel>();
            if (chapter is null)
            {
                return OperationResult<ChapterModel>.Fail("id 错误！");
            }
            if (checkUser && !bookStore.IsSelf(chapter.BookId))
            {
                return OperationResult<ChapterModel>.Fail("id 错误！");
            }
            if (chapter.Type == TYPE_GROUP)
            {
                return OperationResult.Ok(chapter);
            }
            chapter.Content = db.ChapterBodies.Where(i => i.Id == id).Select(i => i.Content).Single();
            return OperationResult.Ok(chapter);
        }

        public IOperationResult<ChapterEntity> Save(ChapterForm data, bool checkUser = false)
        {
            var model = data.Id > 0 ? db.Chapters.Where(i => i.Id == data.Id).Single() : new ChapterEntity();
            if (model is null)
            {
                return OperationResult<ChapterEntity>.Fail("id is error");
            }
            if (checkUser && !bookStore.IsSelf(model.BookId))
            {
                return OperationResult<ChapterEntity>.Fail("id 错误！");
            }
            model.Title = data.Title;
            model.BookId = data.BookId;
            model.Status = data.Status;
            model.Type = data.Type;
            model.ParentId = data.ParentId;
            model.Price = data.Price;
            if (model.Id == 0 && model.Position < 1)
            {
                model.Position = db.Chapters.Where(i => i.BookId == data.BookId).Max(i => i.Position) + 1;
            }
            db.Chapters.Save(model, client.Now);
            if (model.Type != TYPE_GROUP)
            {
                model.Size = data.Content.Length;
            }
            if (db.SaveChanges() == 0)
            {
                throw new Exception("error");
            }
            if (model.Type != TYPE_GROUP)
            {
                var body = db.ChapterBodies.Where(i => i.Id == model.Id).Single();
                if (body is null)
                {
                    db.ChapterBodies.Add(new ChapterBodyEntity()
                    {
                        Id = model.Id,
                        Content = data.Content
                    });
                } else
                {
                    body.Content = data.Content;
                }
                db.SaveChanges();
            }
            bookStore.RefreshSize(model.BookId);
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id, bool checkUser = false)
        {
            var model = db.Chapters.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult.Fail("id 错误！");
            }
            if (checkUser && !bookStore.IsSelf(model.BookId))
            {
                return OperationResult.Fail("id 错误！");
            }
            db.Chapters.Where(i => i.Id == id).ExecuteDelete();
            db.ChapterBodies.Where(i => i.Id == id).ExecuteDelete();
            bookStore.RefreshSize(model.BookId);
            return OperationResult.Ok();
        }

        public IOperationResult<ChapterModel> GetSelf(int id)
        {
            return Get(id, true);
        }

        public IOperationResult<ChapterEntity> SaveSelf(ChapterForm data)
        {
            return Save(data, true);
        }

        public IOperationResult RemoveSelf(int id)
        {
            return Remove(id, true);
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
            var boughtItems = db.
                BuyLogs.Where(i => i.BookId == bookId && i.UserId == client.UserId)
                .Select(i => i.ChapterId).ToArray();
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
            if (client.UserId == 0)
            {
                return false;
            }
            return db.BuyLogs.Where(i => i.BookId == bookId && i.ChapterId == chapterId && i.UserId == client.UserId).Any();
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
                db.Chapters.Where(i => i.BookId == model.BookId && i.Position > model.Position && i.Position < beforeModel.Position)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, i => i.Position - 1));
                db.Chapters.Where(i => i.Id == id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, beforeModel.Position - 1));
                return;
            }
            db.Chapters.Where(i => i.BookId == model.BookId && i.Position < model.Position && i.Position >= beforeModel.Position)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, i => i.Position + 1));
            db.Chapters.Where(i => i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, beforeModel.Position));
        }

        private (ChapterEntity, ChapterEntity) CheckPosition(int id, int twoId)
        {
            if (id == twoId)
            {
                throw new Exception("章节错误");
            }
            var model = db.Chapters.Where(i => i.Id == id).Single();
            if (model is null)
            {
                throw new Exception("章节不存在");
            }
            if (model.Position < 1)
            {
                bookStore.RefreshPosition(model.BookId);
            }
            model.Position = db.Chapters.Where(i => i.Id == id).Select(i => i.Position).Single();
            var twoModel = db.Chapters.Where(i => i.Id == twoId && i.BookId == model.BookId).Single();
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
                db.Chapters.Where(i => i.BookId == model.BookId && i.Position > model.Position && i.Position <= afterModel.Position)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, i => i.Position - 1));
                db.Chapters.Where(i => i.Id == id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, afterModel.Position));
                return;
            }
            db.Chapters.Where(i => i.BookId == model.BookId && i.Position < model.Position && i.Position > afterModel.Position)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, i => i.Position + 1));
            db.Chapters.Where(i => i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Position, afterModel.Position + 1));
        }
    }
}
