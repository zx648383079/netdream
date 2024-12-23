using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class ReportRepository(ContactContext db, IUserRepository userStore,
        IClientContext environment): ISystemFeedback
    {
        public IPage<ReportModel> GetList(string keywords = "", 
            int itemType = 0, int itemId = 0, int type = 0, int page = 1)
        {
            var items = db.Reports.Search(keywords, "title", "email", "content")
                .When(itemType > 0, i => i.ItemType == itemType)
                .When(itemType > 0 && itemId > 0, i => i.ItemId == itemId)
                .When(type > 0, i => i.Type == type)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id)
                .ToPage(page).CopyTo<ReportEntity, ReportModel>();

            userStore.WithUser(items.Items);
            return items;
        }

        public ReportEntity? Get(int id)
        {
            return db.Reports.Where(i => i.Id == id).Single();
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
                UserId = environment.UserId,
                Ip = environment.Ip,
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

        public ReportEntity? Change(int id, int status)
        {
            var model = Get(id);
            if (model is null)
            {
                return null;
            }
            model.Status = status;
            db.Reports.Save(model);
            db.SaveChanges();
            return model;
        }

        public void Remove(params int[] id)
        {
            db.Reports.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }
    }
}
