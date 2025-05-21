using NetDream.Modules.Catering.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class OrderRepository(CateringContext db, IClientContext client)
    {
        public IPage<OrderEntity> GetList(QueryForm form, int store = 0)
        {
            return db.Order.Where(i => i.UserId == client.UserId)
                .When(store > 0, i => i.StoreId == store)
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IPage<OrderEntity> MerchantList(QueryForm form, int user = 0)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.Order.Search(form.Keywords, "name")
            .When(user > 0, i => i.UserId == user)
            .Where(i => i.StoreId == ownStore)
            .OrderByDescending(i => i.Id)
                .ToPage(form);
        }
    }
}
