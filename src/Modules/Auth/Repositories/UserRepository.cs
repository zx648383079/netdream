using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;

namespace NetDream.Modules.Auth.Repositories
{
    public class UserRepository(IDatabase db)
    {
        public const int STATUS_DELETED = 0; // 已删除
        public const int STATUS_FROZEN = 2; // 账户已冻结
        public const int STATUS_UN_CONFIRM = 9; // 邮箱注册，未确认邮箱
        public const int STATUS_ACTIVE = 10; // 账户正常
        public const int STATUS_ACTIVE_VERIFIED = 15; // 账户正常&实名认证了

        public const int SEX_MALE = 1; // 性别男
        public const int SEX_FEMALE = 2; //性别女

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
