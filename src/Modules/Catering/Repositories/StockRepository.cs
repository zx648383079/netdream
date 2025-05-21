using NetDream.Modules.Catering.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class StockRepository(CateringContext db, IClientContext client)
    {
        public IPage<StoreStockEntity> MerchantList(QueryForm form)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.StoreStock.Where(i => i.StoreId == ownStore)
                .ToPage(form);
        }

        public IPage<PurchaseOrderEntity> MerchantOrderList(QueryForm form)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.PurchaseOrder.Where(i => i.StoreId == ownStore)
                .ToPage(form);
        }
    }
}
