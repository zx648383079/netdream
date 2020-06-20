using NetDream.Areas.Contact.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Contact.Repositories
{
    public class ContactRepository
    {
        private readonly IDatabase _db;

        public ContactRepository(IDatabase db)
        {
            _db = db;
        }

        public List<FriendLinkModel> FriendLinks()
        {
            return _db.Fetch<FriendLinkModel>("SELECT * FROM `cif_friend_link` WHERE status = 1");
        }
    }
}
