using Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetDream.Modules.Forum.Repositories
{
    public class ForumRepository(IDatabase db, IUserRepository userStore)
    {
        public Page<ForumEntity> GetList(string keywords = "", 
            int parent = 0, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<ForumEntity>(db).Where("parent_id=@0", parent);
            SearchHelper.Where(sql, "name", keywords);
            return db.Page<ForumEntity>(page, 20, sql);
        }

        public ForumModel? Get(int id, bool full = true)
        {
            var model = db.SingleById<ForumModel>(id);
            if (model is not null && full)
            {
                model.Classifies = db.Fetch<ForumClassifyEntity>("WHERE forum_id=@0 ORDER BY id ASC", id);
                // model.Moderators;
            }
            return model;
        }

        public ForumModel? GetFull(int id, bool full = true)
        {
            var model = db.SingleById<ForumModel>(id);
            if (model is null)
            {
                return model;
            }
            model.Classifies = db.Fetch<ForumClassifyEntity>("WHERE forum_id=@0 ORDER BY id ASC", id);
            if (full)
            {
                //model.Moderators;
                //model.Children = Children(id, false);
                //model.Path = ForumModel.FindPath(id);
                //model.ThreadTop = ThreadRepository.TopList(id);
            }
            return model;
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
            db.Delete<ForumEntity>(id);
        }

        public void All()
        {
            // return ForumModel.Tree().MakeTreeForHtml();
        }

        public void Children(int id, bool hasChildren = true)
        {
            //query = hasChildren ? ForumModel.With("children") : ForumModel.Query();
            //data = query
            //    .Where("parent_id", id).Get();
            //foreach (data as item)
            //{
            //    item.LastThread = static.LastThread(item["id"]);
            //    item.TodayCount = item.GetTodayCountAttribute();
            //    if (hasChildren)
            //    {
            //        foreach (item.Children as it)
            //        {
            //            it.TodayCount = it.GetTodayCountAttribute();
            //            it.LastThread = static.LastThread(it["id"]);
            //        }
            //    }
            //};
            //return data;
        }

        private ThreadModel? LastThread(int id)
        {
            var sql = new Sql();
            sql.Select("id", "title", "user_id", "view_count",
            "post_count",
            "collect_count", MigrationTable.COLUMN_UPDATED_AT)
                .From<ThreadEntity>(db)
                .Where("forum_id=@0", id)
                .OrderBy("id DESC");
            var model = db.Single<ThreadModel>(sql);
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
