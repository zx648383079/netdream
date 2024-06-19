using Modules.Note.Entities;
using NetDream.Core.Extensions;
using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Core.Repositories;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NPoco;

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
            WithUser(items.Items);
            return items;
        }

        private void WithUser(IEnumerable<NoteModel> items)
        {
            var userId = items.Select(item => item.UserId).Where(i => i > 0).Distinct();
            if (!userId.Any())
            {
                return;
            }
            var userItems = userStore.Get(userId.ToArray());
            if (!userItems.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var u in userItems)
                {
                    if (item.UserId == u.Id)
                    {
                        item.User = u;
                        break;
                    }
                }
            }
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
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = environment.Now;
            }
            db.Save(model);
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
            WithUser(items);
            return items;
        }

        public NoteEntity Change(int id, IDictionary<string, string> data)
        {
            var model = db.SingleById<NoteEntity>(id);
            var maps = new string[] { "is_notice" };
            var isUpdated = false;
            foreach (var item in data)
            {
                if (Validator.IsInt(item.Key))
                {
                    if (string.IsNullOrEmpty(item.Value) || !maps.Contains(item.Value))
                    {
                        continue;
                    }
                    var field = model.GetType().GetField(StrHelper.Studly(item.Value));
                    if (field is null)
                    {
                        continue;
                    }
                    if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(model, !(bool)field.GetValue(model));
                    } else
                    {
                        field.SetValue(model, Convert.ChangeType(
                            (byte)field.GetValue(model) > 0 ? 0 : 1, field.FieldType));
                    }
                    isUpdated = true;
                }
                else if(string.IsNullOrEmpty(item.Key) || !maps.Contains(item.Key))
                {
                    var field = model.GetType().GetField(StrHelper.Studly(item.Key));
                    if (field is null)
                    {
                        continue;
                    }
                    field.SetValue(model, Convert.ChangeType(item.Value, field.FieldType));
                    isUpdated = true;
                }
            }
            if (isUpdated)
            {
                db.Update(model);
            }
            return model;
        }
    }
}
