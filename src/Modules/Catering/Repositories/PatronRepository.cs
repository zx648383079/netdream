using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Forms;
using NetDream.Modules.Catering.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class PatronRepository(CateringContext db, 
        IClientContext client, IUserRepository userStore)
    {
        public IPage<PatronListItem> MerchantList(QueryForm form, int group = 0)
        {
            var ownStore = StoreRepository.Own(db, client);
            var res = db.StorePatron.Where(i => i.StoreId == ownStore)
                .When(group > 0, i => i.GroupId == group)
                .OrderByDescending(i => i.UserId)
                .ToPage(form).CopyTo<StorePatronEntity, PatronListItem>();
            userStore.Include(res.Items);
            return res;
        }

        public StorePatronGroupEntity[] MerchantGroupList()
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.StorePatronGroup.Where(i => i.StoreId == ownStore).ToArray();
        }

        public IOperationResult<StorePatronGroupEntity> MerchantGroupSave(PatronGroupForm data)
        {
            var ownStore = StoreRepository.Own(db, client);
            var model = data.Id > 0 ? db.StorePatronGroup
                .Where(i => i.StoreId == ownStore && i.Id == data.Id)
                .FirstOrDefault() : new StorePatronGroupEntity();
            if (model is null)
            {
                return OperationResult<StorePatronGroupEntity>.Fail("error");
            }
            model.Name = data.Name;
            model.Remark = data.Remark;
            model.Discount = data.Discount;
            model.StoreId = ownStore;
            db.StorePatronGroup.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void MerchantGroupRemove(int id)
        {
            var ownStore = StoreRepository.Own(db, client);
            db.StorePatronGroup.Where(
                i => i.StoreId == ownStore && i.Id == id)
                .ExecuteDelete();
            db.StorePatron.Where(i => i.StoreId == ownStore && i.GroupId == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.GroupId, 0));
        }
    }
}
