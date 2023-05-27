using NetDream.Modules.Contact.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Repositories
{
    public class ContactRepository
    {
        private readonly IDatabase _db;

        public ContactRepository(IDatabase db)
        {
            _db = db;
        }

        public List<FriendLinkEntity> FriendLinks()
        {
            return _db.Fetch<FriendLinkEntity>(
                $"SELECT * FROM `{FriendLinkEntity.ND_TABLE_NAME}` WHERE status = 1");
        }
    }
}
