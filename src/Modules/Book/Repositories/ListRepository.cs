using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class ListRepository(BookContext db, 
        IClientContext client,
        IInteractRepository interact,
        HistoryRepository historyStore,
        IUserRepository userStore)
    {
        public IPage<ListEntity> GetList(QueryForm form)
        {
            return db.Lists.Search(form.Keywords, "title")
                .OrderByDescending(i => i.CreatedAt).ToPage(form);
        }

        public IOperationResult<ListModel> Detail(int id)
        {
            var entity = db.Lists.Where(i => i.Id == id).Single();
            if (entity is null)
            {
                return OperationResult<ListModel>.Fail("书单不存在");
            }
            var model = entity.CopyTo<ListModel>();
            model.IsCollected = interact.Has(client.UserId, ModuleTargetType.BookList,
                model.Id, InteractType.Collect);
            var items = db.ListItems.Where(i => i.ListId == id).Select<ListItemEntity, ListItemModel>().ToArray();
            foreach (var item in items)
            {
                item.IsAgree = interact.Has(client.UserId, ModuleTargetType.BookListItem, item.Id, InteractType.Agree);
                item.OnShelf = historyStore.HasBook(item.BookId);
            }
            model.User = userStore.Get(model.UserId);
            model.Items = items;
            entity.ClickCount++;
            db.Lists.Update(entity);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<ListEntity> Save(ListForm data)
        {
            if (data.Items.Count == 0)
            {
                return OperationResult<ListEntity>.Fail("请选择书籍");
            }
            var model = data.Id > 0 ? 
                db.Lists.Where(i => i.Id == data.Id).Single() : new ListEntity() { 
                    UserId = client.UserId,
                };
            if (model is null || model.UserId != client.UserId)
            {
                return OperationResult<ListEntity>.Fail("书单不存在");
            }
            model.Title = data.Title;
            model.Description = data.Description;
            db.Lists.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<ListEntity>.Fail("error");
            }
            var exist = Array.Empty<int>();
            if (data.Id > 0)
            {
                exist = db.ListItems.Where(i => i.ListId == model.Id).Select(i => i.Id)
                    .ToArray();
            }
            var add = new List<ListItemEntity>();
            var update = new List<int>();
            foreach (var item in data.Items)
            {
                if (item.BookId < 1)
                {
                    continue;
                }
                if (item.Id < 1 || !exist.Contains(item.Id))
                {
                    add.Add(new()
                    {
                        ListId = model.Id,
                        BookId = item.BookId,
                        Remark = item.Remark,
                        Star = item.Star < 0 ? 10 : item.Star,
                        CreatedAt = client.Now,
                        UpdatedAt = client.Now,
                    });
                    continue;
                }
                db.ListItems.Where(i => i.ListId == model.Id && i.Id == item.Id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.BookId, item.BookId)
                    .SetProperty(i => i.Remark, item.Remark)
                    .SetProperty(i => i.Star, item.Star));
                update.Add(item.Id);
            }
            var del = exist.Where(i => !update.Contains(i)).ToArray();
            if (del.Length > 0)
            {
                db.ListItems.Where(i => i.ListId == model.Id && del.Contains(i.Id)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.ListItems.AddRange(add);
                db.SaveChanges();
            }
            model.BookCount = db.ListItems.Where(i => i.ListId == model.Id).Count();
            db.Lists.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Lists.Where(i => i.Id == id).Single();
            if (model is null || model.UserId != client.UserId)
            {
                return OperationResult.Fail("书单不存在");
            }
            db.Lists.Remove(model);
            db.ListItems.Where(i => i.ListId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<ListModel> Collect(int id)
        {
            var entity = db.Lists.Where(i => i.Id == id).Single();
            
            if (entity is null)
            {
                return OperationResult<ListModel>.Fail("书单不存在");
            }
            var model = entity.CopyTo<ListModel>();
            var res = interact.Toggle(client.UserId, ModuleTargetType.BookList,
                id, InteractType.Collect);
            if (res)
            {
                model.CollectCount++;
                model.IsCollected = true;
            }
            else
            {
                model.CollectCount--;
                model.IsCollected = false;
            }
            entity.CollectCount = model.CollectCount;
            db.Lists.Save(entity);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<AgreeResult> Agree(int id)
        {
            var model = db.ListItems.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("书单不存在");
            }
            var res = interact.Update(client.UserId, ModuleTargetType.BookList,
                id,
                InteractType.Agree, [InteractType.Agree, InteractType.Disagree]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            switch (res)
            {
                case RecordToggleType.Deleted:
                    data.AgreeCount--;
                    data.AgreeType = AgreeType.None;
                    break;
                case RecordToggleType.Updated:
                    data.AgreeCount++;
                    data.DisagreeCount--;
                    data.AgreeType = AgreeType.Agree;
                    break;
                case RecordToggleType.Added:
                    data.AgreeCount++;
                    data.AgreeType = AgreeType.Agree;
                    break;
            }
            db.ListItems.Where(i => i.Id == model.Id)
                .ExecuteUpdate(setters => setters
                .SetProperty(i => i.AgreeCount, data.AgreeCount)
                .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public IOperationResult<AgreeResult> Disagree(int id)
        {
            var model = db.ListItems.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("书单不存在");
            }
            var res = interact.Update(client.UserId, ModuleTargetType.BookList,
                id,
                InteractType.Disagree, [InteractType.Agree, InteractType.Disagree]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            switch (res)
            {
                case RecordToggleType.Deleted:
                    data.DisagreeCount--;
                    data.AgreeType = AgreeType.None;
                    break;
                case RecordToggleType.Updated:
                    data.AgreeCount--;
                    data.DisagreeCount++;
                    data.AgreeType = AgreeType.Disagree;
                    break;
                case RecordToggleType.Added:
                    data.DisagreeCount++;
                    data.AgreeType = AgreeType.Disagree;
                    break;
            }
            db.ListItems.Where(i => i.Id == model.Id)
                   .ExecuteUpdate(setters => setters
                   .SetProperty(i => i.AgreeCount, data.AgreeCount)
                   .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

    }
}
