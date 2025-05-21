using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class AddressRepository(CateringContext db, IClientContext client)
    {
        public IPage<AddressListItem> GetList(QueryForm form)
        {
            return db.Address.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToPage(form).CopyTo<AddressEntity, AddressListItem>();
        }
    }
}
