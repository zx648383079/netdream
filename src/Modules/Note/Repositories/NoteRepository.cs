using Modules.Note.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NPoco;
using System.Collections.Generic;
using System;

namespace NetDream.Modules.Note.Repositories
{
    public class NoteRepository(IDatabase db, 
        IClientEnvironment environment, IUserRepository userStore,
        LocalizeRepository localize)
    {
        public const int STATUS_VISIBLE = 1;
        public const int STATUS_HIDE = 0;

        public Page<NoteEntity> GetManageList(string keywords = "", int user = 0, long page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "content", keywords);
            if (user > 0)
            {
                sql.Where("user_id=@0", user);
            }
            sql.OrderBy("id DESC");
            return db.Page<NoteEntity>(page, 20, sql);
        }

        public Page<NoteModel> GetList(string keywords = "", int user = 0, int id = 0, bool notice = false, long page = 1, int perPage = 20)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "content", keywords);
            if (id > 0)
            {
                sql.Where("id=@0", id);
            }
            if (notice)
            {
                sql.Where("is_notice=1");
            }
            if (user > 0)
            {
                sql.Where("user_id=@0", user);
            }
            sql.Where("status=@0", STATUS_VISIBLE);
            if (environment.UserId > 0)
            {
                sql.Append("OR user_id=@0", environment.UserId);
            }
            sql.OrderBy("id DESC");
            var items = db.Page<NoteModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public NoteModel? Get(int id)
        {
            var model = db.SingleById<NoteModel>(id);
            if (model is not null)
            {
                model.User = userStore.Get(model.UserId);
            }
            return model;
        }

        public NoteModel? GetSelf(int id)
        {
            var model = db.Single<NoteModel>("WHERE user_id=@0 AND id=@1", environment.UserId, id);
            if (model is not null)
            {
                model.User = userStore.Get(model.UserId);
            }
            return model;
        }

        public NoteEntity Save(NoteForm data, int userId = 0)
        {
            var model = data.Id > 0 ? db.SingleById<NoteEntity>(data.Id) : 
                new NoteEntity();
            if (data.Id > 0 && userId > 0 && model.UserId != userId)
            {
                throw new Exception("note error");
            }
            model.Content = data.Content;
            if (userId > 0 || model.UserId == 0)
            {
                model.UserId = userId;
            }
            db.TrySave(model);
            return model;
        }

        public NoteEntity SaveSelf(NoteForm data)
        {
            return Save(data, environment.UserId);
        }

        public void Remove(int id)
        {
            db.Delete<NoteEntity>(id);
        }

        public void RemoveSelf(int id)
        {
            db.Delete<NoteEntity>("WHERE user_id=@0 AND id=@1", environment.UserId, id);
        }

        public IList<string> Suggestion(string keywords)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "content", keywords);
            sql.Limit(4);
            return db.Pluck<string>(sql, "content");
        }

        public IList<NoteModel> GetNewList(int limit = 5)
        {
            var sql = new Sql();
            sql.Where("is_notice=@0", localize.BrowserLanguageIsDefault ? 2 : 1);
            sql.OrderBy("id DESC");
            sql.Limit(limit > 0 ? limit : 5);
            var items = db.Fetch<NoteModel>(sql);
            userStore.WithUser(items);
            return items;
        }

        public NoteEntity? Change(int id, IDictionary<string, string> data)
        {
            return ModelHelper.BatchToggle<NoteEntity>(db, id, data, ["is_notice"]);
        }
    }
}
