using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetDream.Modules.Note.Repositories
{
    public class NoteRepository(NoteContext db, 
        IClientContext client, IUserRepository userStore,
        LocalizeRepository localize)
    {
        public const int STATUS_VISIBLE = 1;
        public const int STATUS_HIDE = 0;

        public IPage<NoteListItem> ManageList(NoteQueryForm form)
        {
            var items = db.Notes.Search(form.Keywords, "content")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }
        public IPage<NoteListItem> GetList(NoteQueryForm form)
        {
            var query = db.Notes.Search(form.Keywords, "content")
                .When(form.Id > 0, i => i.Id == form.Id)
                .When(form.Notice > 0, i => i.IsNotice == 1)
                .When(form.User > 0, i => i.UserId == form.User);
            if (client.UserId > 0)
            {
                query = query.Where(i => i.Status == STATUS_VISIBLE || i.UserId == client.UserId);
            } else
            {
                query = query.Where(i => i.Status == STATUS_VISIBLE);
            }
            var items = query.OrderByDescending(i => i.Id).ToPage(form, i => i.SelectAs());
            foreach (var item in items.Items)
            {
                item.Html = RenderHtml(item.Html);
            }
            userStore.Include(items.Items);
            return items;
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
            var model = db.Notes.Where(i => i.Id == id && i.UserId == client.UserId).Single()?.CopyTo<NoteModel>();
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
            if (data.Id == 0 && userId == 0)
            {
                model.UserId = client.UserId;
            }
            db.Notes.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<NoteEntity> SelfSave(NoteForm data)
        {
            return Save(data, client.UserId);
        }

        public void Remove(int id)
        {
            db.Notes.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void SelfRemove(int id)
        {
            db.Notes.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
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
            userStore.Include(items);
            return items;
        }

        public IOperationResult<NoteEntity> Change(int id, IDictionary<string, string> data)
        {
            var res = db.Notes.BatchToggle(id, data, "is_notice");
            if (res is null)
            {
                return OperationResult.Fail<NoteEntity>("数据错误");
            }
            db.SaveChanges();
            return OperationResult.Ok(res);
        }

        internal static string RenderHtml(string content)
        {
            var res = HttpUtility.HtmlEncode(content).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                .Replace(" ", "&nbsp;");
            return string.Join("", res.Split('\n').Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => $"<p>{i}</p>"));
        }

    }
}
