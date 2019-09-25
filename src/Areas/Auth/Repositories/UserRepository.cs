using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Auth.Repositories
{
    public class UserRepository
    {
        private readonly IDatabase _db;

        public UserRepository(IDatabase db)
        {
            _db = db;
        }
    }
}
