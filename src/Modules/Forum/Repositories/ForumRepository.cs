using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Forum.Repositories
{
    public class ForumRepository(ForumContext db, 
        IUserRepository userStore,
        IClientContext client)
    {
        public IPage<ForumEntity> GetList(ForumQueryForm form)
        {
            return db.Forums.Search(form.Keywords, "name")
                .Where(i => i.ParentId == form.Parent)
                .ToPage(form);
        }

        public IOperationResult<ForumModel> Get(int id, bool full = true)
        {
            var model = db.Forums.Where(i => i.Id == id).Single()?.CopyTo<ForumModel>();
            if (model is null)
            {
                return OperationResult.Fail<ForumModel>("id is error");
            }
            if (full)
            {
                model.Classifies = db.ForumClassifies.Where(i => i.ForumId == id)
                    .OrderBy(i => i.Id).ToArray();
                model.Moderators = db.ForumModerators.Where(i => i.ForumId == id)
                    .OrderBy(i => i.RoleId).SelectAs().ToArray();
                userStore.Include(model.Moderators);
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ForumModel> GetFull(int id, bool full = true)
        {
            var model = db.Forums.Where(i => i.Id == id).Single()?.CopyTo<ForumModel>();
            if (model is null)
            {
                return OperationResult<ForumModel>.Fail("id is error");
            }
            model.Classifies = db.ForumClassifies.Where(i => i.ForumId == id).OrderBy(i => i.Id).ToArray();
            if (full)
            {
                model.Moderators = db.ForumModerators.Where(i => i.ForumId == id)
                    .OrderBy(i => i.RoleId).SelectAs().ToArray();
                userStore.Include(model.Moderators);
                model.Children = Children(id, false);
                //model.Path = ForumModel.FindPath(id);
                model.ThreadTop = new ThreadRepository(db, userStore, client, null).TopList(id);
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ForumEntity> Save(ForumForm data)
        {
            var model = data.Id > 0 ? db.Forums.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new ForumEntity();
            if (model is null)
            {
                return OperationResult.Fail<ForumEntity>("id error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Thumb = data.Thumb;
            model.ParentId = data.ParentId;
            model.Position = data.Position;
            model.Type = data.Type;
            db.Forums.Save(model, client.Now);
            db.SaveChanges();
            if (data.Classifies?.Length > 0)
            {
                foreach (var item in data.Classifies)
                {
                    item.ForumId = model.Id;
                    SaveClassify(item);
                }
            }
            if (data.Moderators?.Length > 0)
            {
                SaveModerator(data.Moderators, model.Id);
            }
            return OperationResult.Ok(model);
        }
        public void SaveModerator(ModeratorForm[] items, int forumId)
        {
            var exist = db.ForumModerators.Where(i => i.ForumId == forumId)
                .Pluck(i => i.UserId);
            var users = items.Select(i => i.UserId).ToArray();
            var remove = ModelHelper.Diff(exist, users);
            if (remove.Length > 0)
            {
                db.ForumModerators.Where(i => i.ForumId == forumId && remove.Contains(i.UserId))
                    .ExecuteDelete();
            }
            var updated = new HashSet<int>();
            foreach (var item in items)
            {
                if (updated.Contains(item.UserId) || item.UserId < 1)
                {
                    continue;
                }
                updated.Add(item.UserId);
                if (exist.Contains(item.UserId))
                {
                    db.ForumModerators.Where(i => i.ForumId == forumId && i.UserId == item.UserId)
                        .ExecuteUpdate(setters => setters.SetProperty(i => i.RoleId, item.RoleId));
                    continue;
                }
                db.ForumModerators.Add(new ForumModeratorEntity()
                {
                    ForumId = forumId,
                    UserId = item.UserId,
                    RoleId = item.RoleId,
                });
            }
            db.SaveChanges();
        }
        public void SaveModerator(int[] users, int forumId)
        {
            var exist = db.ForumModerators.Where(i => i.ForumId == forumId)
                .Pluck(i => i.UserId);
            var add = ModelHelper.Diff(users, exist);
            var remove = ModelHelper.Diff(exist, users);
            if (remove.Length > 0)
            {
                db.ForumModerators.Where(i => i.ForumId == forumId && remove.Contains(i.UserId))
                    .ExecuteDelete();
            }
            if (add.Length > 0)
            {
                db.ForumModerators.AddRange(add.Select(i => new ForumModeratorEntity()
                {
                    UserId = i,
                    ForumId = forumId
                }));
            }
            db.SaveChanges();
        }

        public IOperationResult<ForumClassifyEntity> SaveClassify(ClassifyForm data)
        {
            var model = data.Id > 0 ? db.ForumClassifies.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new ForumClassifyEntity();
            if (model is null)
            {
                return OperationResult.Fail<ForumClassifyEntity>("id error");
            }
            model.Name = data.Name;
            model.Icon = data.Icon;
            model.ForumId = data.ForumId;
            model.Position = data.Position;
            db.ForumClassifies.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Forums.Where(i => i.Id == id).ExecuteDelete();
        }

        public ForumTreeItem[] All()
        {
            var data = db.Forums
                .OrderBy(i => i.ParentId)
                .ThenBy(i => i.Position)
                .Select(i => new ForumTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).ToArray();
            return TreeHelper.Sort(data);
        }

        public ForumListItem[] Children(int id, bool hasChildren = true)
        {
            var res = db.Forums.Where(i => i.ParentId == id)
                .SelectAs().ToArray();
            if (res.Length == 0)
            {
                return res;
            }
            if (hasChildren)
            {
                IncludeChildren(res);
            }
            IncludeTodayCount(res);
            return res;
        }

        private void IncludeChildren(ForumListItem[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.Forums.Where(i => idItems.Contains(i.ParentId)).SelectAs().ToArray();
            IncludeTodayCount(data);
            foreach (var item in items)
            {
                item.Children = data.Where(i => i.ParentId == item.Id).ToArray();
            }
        }

        private void IncludeTodayCount(ForumListItem[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var today = TimeHelper.TimestampFrom(DateTime.Today);
            var todayEnd = today + 86400;
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.Threads.Where(i => idItems.Contains(i.ForumId) && i.CreatedAt >= today && i.CreatedAt < todayEnd)
                .GroupBy(i => i.ForumId)
                .Select(i => new KeyValuePair<int, int>(i.Key, i.Count()))
                .ToDictionary();
            foreach (var item in items)
            {
                if (data.TryGetValue(item.Id, out var count))
                {
                    item.TodayCount = count;
                }
            }
        }

        private ThreadModel? LastThread(int id)
        {
            var model = db.Threads.Where(i => i.ForumId == id)
                .OrderByDescending(i => i.Id)
                .Select(i => new ThreadModel()
                {
                    Id = i.Id,
                    Title = i.Title,
                    UserId = i.UserId,
                    ViewCount = i.ViewCount,
                    PostCount = i.PostCount,
                    CollectCount = i.CollectCount,
                    UpdatedAt = i.UpdatedAt,
                }).Single();
            if (model is not null)
            {
                model.User = userStore.Get(id);
            }
            return model;
        }

        public void UpdateCount(int id, string key = "thread_count", int count = 1)
        {
            //path = TreeHelper.GetTreeParent(ForumModel.CacheAll(), id);
            //path[] = id;
            //return ForumModel.WhereIn("id", path)
            //    .UpdateIncrement(key, count);
        }
    }
}
