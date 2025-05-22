using NetDream.Modules.Legwork.Models;
using NetDream.Shared.Interfaces;
using System;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class LegworkRepository(LegworkContext db, IClientContext client)
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_ALLOW = 1;
        public const byte STATUS_DISALLOW = 2;
        public RoleStatus Role()
        {
            var user_id = client.UserId;
            return new()
            {
                IsProvider = db.Provider.Where(i => i.UserId == user_id && i.Status > 0).Any(),
                IsWaiter = db.Waiter.Where(i => i.UserId == user_id && i.Status > 0).Any(),
            };
        }

        public int WaitTaking()
        {
            return db.Order.Where(i => i.WaiterId == 0 && i.Status == OrderRepository.STATUS_PAID_UN_TAKING)
                .Count();
        }
    }
}
