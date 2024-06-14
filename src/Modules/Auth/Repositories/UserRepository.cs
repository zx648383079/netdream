using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;

namespace NetDream.Modules.Auth.Repositories
{
    public class UserRepository(IDatabase db)
    {
        public UserModel? Login(string email, string password)
        {
            var user = db.Single<UserEntity>("where email=@0", email);
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
