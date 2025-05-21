using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class CategoryRepository(CateringContext db, IClientContext client)
    {
        public const byte TYPE_PRODUCT = 0;
        public const byte TYPE_RECIPE = 9;
        public const byte TYPE_STOCK = 4;

        public CategoryEntity[] GetList(int store)
        {
            return db.Category.Where(i => i.StoreId == store && i.Type == TYPE_PRODUCT
                && i.ParentId == 0).ToArray();
        }

        public CategoryEntity[] MerchantList(int type)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.Category.Where(i => i.StoreId == ownStore && i.Type == type && i.ParentId == 0)
                .ToArray();
        }

        public IOperationResult<CategoryEntity> MerchantSave(byte type, CategoryForm data)
        {
            var ownStore = StoreRepository.Own(db, client);
            var model = data.Id > 0 ? db.Category.Where(i => i.Id == data.Id && i.StoreId == ownStore)
                .FirstOrDefault() : new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("error");
            }
            model.Name = data.Name;
            model.ParentId = data.ParentId;
            model.Type = type;
            model.StoreId = ownStore;
            db.Category.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void MerchantRemove(int type, int id)
        {
            var ownStore = StoreRepository.Own(db, client);
            db.Category.Where(i => i.Id == id && i.StoreId == ownStore)
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
