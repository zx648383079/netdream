using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Forms;
using NetDream.Modules.Catering.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Data;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class StaffRepository(
        CateringContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<StaffListItem> MerchantList(QueryForm form, int role = 0)
        {
            var ownStore = StoreRepository.Own(db, client);
            var res = db.StoreStaff.Where(i => i.StoreId == ownStore)
                .When(role > 0, i => i.RoleId == role)
                .OrderByDescending(i => i.UserId)
                .ToPage(form).CopyTo<StoreStaffEntity, StaffListItem>();
            userStore.Include(res.Items);
            return res;
        }

        public StoreRoleEntity[] MerchantRoleList()
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.StoreRole.Where(i => i.StoreId == ownStore).ToArray();
        }

        public IOperationResult<StoreRoleEntity> MerchantRoleSave(RoleForm data)
        {
            var ownStore = StoreRepository.Own(db, client);
            var model = data.Id > 0 ? db.StoreRole.Where(i => i.Id == data.Id && i.StoreId == ownStore)
                .FirstOrDefault() : new StoreRoleEntity();
            if (model is null)
            {
                return OperationResult<StoreRoleEntity>.Fail("error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Action = data.Action;
            model.StoreId = ownStore;
            db.StoreRole.Save(model, client.Now);
            return OperationResult.Ok(model);
        }

        public void MerchantRoleRemove(int id)
        {
            var ownStore = StoreRepository.Own(db, client);
            db.StoreRole.Where(i => i.StoreId == ownStore && i.Id == id)
                .ExecuteDelete();
            db.StoreStaff.Where(i => i.StoreId == ownStore && i.RoleId == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.RoleId, 0)); ;
        }
    }
}
