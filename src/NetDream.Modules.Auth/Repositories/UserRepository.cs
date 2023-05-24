using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Repositories
{
    public class UserRepository
    {
        private readonly IDatabase _db;

        public UserRepository(IDatabase db)
        {
            _db = db;
        }

        public UserModel? Login(string email, string password)
        {
            var user = _db.Single<UserEntity>("where email=@0", email);
            if (user == null)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return new UserModel()
            {
                Id = user.Id,
            };
        }
    }
}
