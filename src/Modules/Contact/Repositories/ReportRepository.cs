using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class ReportRepository(ContactContext db, IUserRepository userStore,
        IClientContext client): ISystemFeedback
    {
        public IPage<ReportListItem> GetList(ReportQueryForm form)
        {
            var items = db.Reports.Search(form.Keywords, "title", "email", "content")
                .When(form.ItemType > 0, i => i.ItemType == form.ItemType)
                .When(form.ItemType > 0 && form.ItemId > 0, i => i.ItemId == form.ItemId)
                .When(form.Type > 0, i => i.Type == form.Type)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IOperationResult<ReportEntity> Get(int id)
        {
            var model = db.Reports.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ReportEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ReportEntity> Create(ReportForm data)
        {
            var model = new ReportEntity()
            {
                ItemType = data.ItemType,
                ItemId = data.ItemId,
                Type = data.Type,
                Title = data.Title,
                Content = data.Content,
                Email = data.Email,
                Files = data.Files,
                UserId = client.UserId,
                Ip = client.Ip,
            };
            if (!Check(model)) {
                return OperationResult.Fail<ReportEntity>("请不要重复操作");
            }
            db.Reports.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public bool Check(ReportEntity model)
        {
            return !db.Reports.Where(i => i.ItemId == model.ItemId && i.ItemType == model.ItemType && i.Status == 0)
                .When(model.UserId > 0, i => i.UserId == model.UserId, i => i.Ip == model.Ip)
                .Any();
        }
        public IOperationResult<ReportEntity> QuickCreate(byte itemType, int itemId,
            string content, string title)
        {
            return Create(new ReportForm()
            {
                ItemType = itemType,
                ItemId = itemId,
                Content = content,
                Type = 99,
                Title = title
            });
        }
        public IOperationResult<ReportEntity> QuickCreate(byte itemType, int itemId, 
            string content, byte type = 99)
        {
            return Create(new ReportForm()
            {
                ItemType = itemType,
                ItemId = itemId,
                Content = content,
                Type = type,
                Title = "其他投诉/举报"
            });
        }

        public int Report(byte itemType, int itemId, string content, string title)
        {
            var model = QuickCreate(itemType, itemId, content, title);
            return model.Succeeded ? model.Result.Id : 0;
        }

        public IOperationResult<ReportEntity> Change(int id, int status)
        {
            var model = db.Reports.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ReportEntity>.Fail("数据错误");
            }
            model.Status = status;
            db.Reports.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(params int[] id)
        {
            db.Reports.Where(i => id.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
