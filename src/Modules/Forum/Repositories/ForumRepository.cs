using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Forum.Repositories
{
    public class ForumRepository(ForumContext db, IUserRepository userStore)
    {
        public IPage<ForumEntity> GetList(string keywords = "", 
            int parent = 0, int page = 1)
        {
            return db.Forums.Search(keywords, "name")
                .Where(i => i.ParentId == parent)
                .ToPage(page);
        }

        public ForumModel? Get(int id, bool full = true)
        {
            var model = db.Forums.Where(i => i.Id == id).Single()?.CopyTo<ForumModel>();
            if (model is not null && full)
            {
                model.Classifies = db.ForumClassifies.Where(i => i.ForumId == id).OrderBy(i => i.Id).ToArray();
                // model.Moderators;
            }
            return model;
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
                    .OrderBy(i => i.RoleId).ToArray();
                //model.Children = Children(id, false);
                //model.Path = ForumModel.FindPath(id);
                //model.ThreadTop = ThreadRepository.TopList(id);
            }
            return OperationResult.Ok(model);
        }

        public ForumEntity Save(ForumForm data)
        {
            //id = data["id"] ?? 0;
            //unset(data["id"]);
            //model = ForumModel.FindOrNew(id);
            //model.Load(data);
            //if (!model.Save())
            //{
            //    throw new Exception(model.GetFirstError());
            //}
            //if (isset(data["classifies"]))
            //{
            //    foreach (data["classifies"] as item)
            //    {
            //        item["forum_id"] = model.Id;
            //        SaveClassify(item);
            //    }
            //}
            //if (isset(data["moderators"]))
            //{
            //    SaveModerator(array_column(data["moderators"], "id"), model.Id);
            //}
            //return model;
            return null;
        }

        //public void SaveModerator(array users, int forum_id)
        //{
        //    exist = ForumModeratorModel.Where("forum_id", forum_id)
        //        .Pluck("user_id");
        //    add = array_diff(users, exist);
        //    remove = array_diff(exist, users);
        //    if (!empty(add))
        //    {
        //        ForumModeratorModel.Query().Insert(array_map(use(forum_id)(object user_id) {
        //            return compact("user_id", "forum_id");
        //        }, add));
        //    }
        //    if (!empty(remove))
        //    {
        //        ForumModeratorModel.Where("forum_id", forum_id)
        //            .WhereIn("user_id", remove).Delete();
        //    }
        //}

        //public void SaveClassify(array data)
        //{
        //    id = data["id"] ?? 0;
        //    unset(data["id"]);
        //    model = ForumClassifyModel.FindOrNew(id);
        //    model.Load(data);
        //    if (!model.Save())
        //    {
        //        throw new Exception(model.GetFirstError());
        //    }
        //    return model;
        //}

        public void Remove(int id)
        {
            db.Forums.Where(i => i.Id == id).ExecuteDelete();
        }

        public void All()
        {
            // return ForumModel.Tree().MakeTreeForHtml();
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
