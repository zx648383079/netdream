using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class ApplyRepository(UserContext db,
        IUserRepository userStore,
        IClientContext client): IApplyRepository
    {

        public IPage<IApplyListItem> GetList(LogQueryForm form)
        {
            var items = db.ApplyLogs.Search(form.Keywords, "remark")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IOperationResult<ApplyLogEntity> Change(int id, byte status)
        {
            var model = db.ApplyLogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null) 
            {
                return OperationResult<ApplyLogEntity>.Fail("id is error");
            }
            if (model.Status == status)
            {
                return OperationResult.Ok(model);
            }
            model.Status = status;
            db.ApplyLogs.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }


        public void ReceiveCancel(int user, ModuleTargetType itemType, int itemId)
        {
            db.ApplyLogs.Where(i => i.UserId == user && i.ItemType == (byte)itemType && i.ItemType == itemId && i.Status == (byte)ReviewStatus.None)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, (byte)ReviewStatus.Deleted)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            db.SaveChanges();
        }

        public void ReceiveClear(ModuleTargetType itemType, int itemId)
        {
            db.ApplyLogs.Where(i => i.ItemType == (byte)itemType && i.ItemType == itemId && i.Status == (byte)ReviewStatus.None)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, (byte)ReviewStatus.Deleted)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            db.SaveChanges();
        }

        public void Receive(int user, ModuleTargetType itemType, int itemId, ReviewStatus status)
        {
            db.ApplyLogs.Where(i => i.UserId == user && i.ItemType == (byte)itemType 
                    && i.ItemType == itemId && i.Status == (byte)ReviewStatus.None)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, (byte)status)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            db.SaveChanges();
        }

        public void ReceiveCreate(int user, ModuleTargetType itemType, int itemId, string remark)
        {
            db.ApplyLogs.Save(new ApplyLogEntity()
            {
                UserId = user,
                ItemType = (byte)itemType,
                ItemId = itemId,
                Remark = remark,
                Status = (byte)ReviewStatus.None
            });
            db.SaveChanges();
        }

        public IPage<IApplyListItem> ReceiveSearch(ModuleTargetType itemType, int itemId, PaginationForm form)
        {
            var items = db.ApplyLogs.Where(i => i.ItemType == (byte)itemType && i.ItemId == itemId)
                .OrderBy(i => i.Status)
                .ThenByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public int ReceiveUnread(ModuleTargetType itemType, int itemId, int lastAt = 0)
        {
            return db.ApplyLogs.Where(i => i.ItemType == (byte)itemType && i.ItemId == itemId
                && i.Status == (byte)ReviewStatus.None)
                .When(lastAt > 0, i => i.CreatedAt > lastAt).Count();
        }

        public bool ReceiveAny(int user, ModuleTargetType itemType, int itemId)
        {
            return db.ApplyLogs.Where(i => i.ItemType == (byte)itemType && i.ItemId == itemId
                && i.UserId == user).Any();
        }

        public IPage<IApplyListItem> ReceiveSearch(ModuleTargetType type, int target, IPaginationForm form)
        {
            throw new NotImplementedException();
        }
    }
}
