using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class ApplyRepository(
        ChatContext db,
        IClientContext client,
        IUserRepository userStore,
        GroupRepository groupStore
        )
    {
        public IPage<ApplyModel> GroupList(int id = 0, int page = 1)
        {
            if (!groupStore.Manageable(id))
            {
                // throw new Exception("无权限处理");
                return new Page<ApplyModel>();
            }
            var items = db.Applies.Where(i => i.ItemType == 1 && i.ItemId == id)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page).CopyTo<ApplyEntity, ApplyModel>();
            userStore.Include(items.Items);
            return items;
        }

        public IPage<ApplyModel> GetList(int page = 1)
        {
            var items = db.Applies.Where(i => i.ItemType == 0 && i.ItemId == client.UserId)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page).CopyTo<ApplyEntity, ApplyModel>();
            userStore.Include(items.Items);
            return items;
        }

        public void RemoveMy()
        {
            db.Applies.Where(i => i.ItemType == 0 && i.ItemId == client.UserId).ExecuteDelete();
        }

        public IOperationResult RemoveGroup(int id)
        {
            if (!groupStore.Manageable(id))
            {
                return OperationResult.Fail("无权限处理");
            }
            db.Applies.Where(i => i.ItemType == 0 && i.ItemId == id).ExecuteDelete(); ;
            return OperationResult.Ok();
        }

        public void Agree(int user)
        {
            db.Applies.Where(i => i.UserId == user && i.ItemType == 0 && i.ItemId == client.UserId && i.Status == 0)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1)
                .SetProperty(i => i.UpdatedAt, client.Now));
        }

        public void AgreeGroup(int user, int id)
        {
            db.Applies.Where(i => i.UserId == user && i.ItemType == 1 && i.ItemId == id && i.Status == 0)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1)
                .SetProperty(i => i.UpdatedAt, client.Now));
        }

        public void Apply(int type, int id, string remark = "")
        {
            db.Applies.Save(new ApplyEntity()
            {
                ItemType = type,
                ItemId = id,
                Remark = remark,
                UserId = client.UserId,
                Status = 0
            }, client.Now);
        }
    }
}
