using NetDream.Areas.Auth.Models;
using NPoco;

namespace NetDream.Areas.Auth.Repositories
{
    public class UserRepository
    {
        private readonly IDatabase _db;

        public UserRepository(IDatabase db)
        {
            _db = db;
        }

        public UserModel Login(string email, string password)
        {
            var user = _db.Single<UserModel>("where email=@0", email);
            if (user == null)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return user;
        }
    }
}
