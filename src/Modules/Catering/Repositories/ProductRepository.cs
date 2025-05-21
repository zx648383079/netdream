using NetDream.Modules.Catering.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class ProductRepository(CateringContext db, IClientContext client)
    {
        public IPage<GoodsEntity> GetList(QueryForm form, int store, int category = 0)
        {
            return db.Goods.Where(i => i.StoreId == store)
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .ToPage(form);
        }

        public IOperationResult<GoodsEntity> Get(int store, int id)
        {
            var model = db.Goods.Where(i => i.StoreId == store && i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsEntity>.Fail("product is error");
            }
            return OperationResult.Ok(model);
        }

        public IPage<GoodsEntity> MerchantList(QueryForm form, int category = 0)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.Goods.Where(i => i.StoreId == ownStore)
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .ToPage(form);
        }

        public IOperationResult<GoodsEntity> MerchantGet(int id)
        {
            var ownStore = StoreRepository.Own(db, client);
            var model = db.Goods.Where(i => i.StoreId == ownStore && i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsEntity>.Fail("product is error");
            }
            return OperationResult.Ok(model);
        }
    }
}
