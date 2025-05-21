using NetDream.Modules.Catering.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class RecipeRepository(CateringContext db, IClientContext client)
    {
        public IPage<RecipeEntity> MerchantList(QueryForm form)
        {
            var ownStore = StoreRepository.Own(db, client);
            return db.Recipe.Where(i => i.StoreId == ownStore)
                .Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }
    }
}
