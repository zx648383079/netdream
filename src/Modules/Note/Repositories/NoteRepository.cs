using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetDream.Modules.Note.Repositories
{
    public class NoteRepository(NoteContext db, 
        IClientContext environment, IUserRepository userStore,
        LocalizeRepository localize)
    {
        public const int STATUS_VISIBLE = 1;
        public const int STATUS_HIDE = 0;

        public IPage<NoteEntity> GetManageList(string keywords = "", 
            int user = 0, int page = 1)
        {
            return db.Notes.Search(keywords, "content")
                .When(user > 0, i => i.UserId == user).OrderByDescending(i => i.Id)
                .ToPage(page);
        }
        public IPage<NoteListItem> GetList(QueryForm form,
            int user = 0,
            int id = 0, bool notice = false)
        {
            var query = db.Notes.Search(form.Keywords, "content")
                .When(id > 0, i => i.Id == id)
                .When(notice, i => i.IsNotice == 1)
                .When(user > 0, i => i.UserId == user);
            if (environment.UserId > 0)
            {
                query = query.Where(i => i.Status == STATUS_VISIBLE || i.UserId == environment.UserId);
            } else
            {
                query = query.Where(i => i.Status == STATUS_VISIBLE);
            }
            var items = query.OrderByDescending(i => i.Id).ToPage(form);
            var res = new Page<NoteListItem>(items)
            {
                Items = items.Items.Select(i => new NoteListItem(i)).ToArray()
            };
            userStore.WithUser(res.Items);
            return res;
        }

        public NoteModel? Get(int id)
        {
            var model = db.Notes.Where(i => i.Id == id).Single()?.CopyTo<NoteModel>();
            if (model is null)
            {
                return null;
            }
            model.User = userStore.Get(model.UserId);
            return model;
        }

        public NoteModel? GetSelf(int id)
        {
            var model = db.Notes.Where(i => i.Id == id && i.UserId == environment.UserId).Single()?.CopyTo<NoteModel>();
            if (model is not null)
            {
                model.User = userStore.Get(model.UserId);
            }
            return model;
        }

        public IOperationResult<NoteEntity> Save(NoteForm data, int userId = 0)
        {
            var model = data.Id > 0 ? db.Notes.Where(i => i.Id == data.Id).Single() : 
                new NoteEntity();
            if (data.Id > 0 && userId > 0 && model.UserId != userId)
            {
                return OperationResult.Fail<NoteEntity>("note error");
            }
            model.Content = data.Content;
            if (userId > 0 || model.UserId == 0)
            {
                model.UserId = userId;
            }
            db.Notes.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<NoteEntity> SaveSelf(NoteForm data)
        {
            return Save(data, environment.UserId);
        }

        public void Remove(int id)
        {
            db.Notes.Where(i => i.Id == id).ExecuteDelete();
        }

        public void RemoveSelf(int id)
        {
            db.Notes.Where(i => i.Id == id && i.UserId == environment.UserId).ExecuteDelete();
        }

        public string[] Suggestion(string keywords)
        {
            return db.Notes.Search(keywords, "content")
                .Take(4).Select(i => i.Content).ToArray();
        }

        public NoteModel[] GetNewList(int limit = 5)
        {
            var notice = localize.BrowserLanguageIsDefault ? 2 : 1;
            var items = db.Notes.Where(i => i.IsNotice == notice)
                .OrderByDescending(i => i.Id).Take(limit > 0 ? limit : 5)
                .ToArray().Select(i => i.CopyTo<NoteModel>()).ToArray();
            userStore.WithUser(items);
            return items;
        }

        public NoteEntity? Change(int id, IDictionary<string, string> data)
        {
            var res = db.Notes.BatchToggle(id, data, "is_notice");
            if (res is null)
            {
                return null;
            }
            db.SaveChanges();
            return res;
        }

        internal static string RenderHtml(string content)
        {
            var res = WebUtility.HtmlEncode(content).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                .Replace(" ", "&nbsp;");
            return string.Join("", res.Split('\n').Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => $"<p>{i}</p>"));
        }
    }
}
