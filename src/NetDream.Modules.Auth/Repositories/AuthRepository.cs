using Google.Protobuf.WellKnownTypes;
using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;

namespace NetDream.Modules.Auth.Repositories
{
    public class AuthRepository
    {
        const byte ACCOUNT_TYPE_NAME = 1;
        const byte ACCOUNT_TYPE_EMAIL = 2;
        const byte ACCOUNT_TYPE_MOBILE = 3;
        const byte ACCOUNT_TYPE_OAUTH_QQ = 11;
        const byte ACCOUNT_TYPE_OAUTH_WX = 12;
        const byte ACCOUNT_TYPE_OAUTH_WX_MINI = 13;
        const byte ACCOUNT_TYPE_OAUTH_WEIBO = 14;
        const byte ACCOUNT_TYPE_OAUTH_TAOBAO = 15;
        const byte ACCOUNT_TYPE_OAUTH_ALIPAY = 16;
        const byte ACCOUNT_TYPE_OAUTH_GITHUB = 21;
        const byte ACCOUNT_TYPE_OAUTH_GOOGLE = 21;
        const byte ACCOUNT_TYPE_ID_CARD = 98;
        const byte ACCOUNT_TYPE_IP = 99;

        const string UNSET_PASSWORD = "no_password";
        const string OPTION_REGISTER_CODE = "auth_register";

        private readonly IDatabase _db;
        private readonly IGlobeOption _option;

        public AuthRepository(IDatabase db, IGlobeOption option)
        {
            _db = db;
            _option = option;
        }

        public AuthRegisterType RegisterType {
            get {
                var val = _option.Get<int>(OPTION_REGISTER_CODE);
                return (AuthRegisterType)val;
            }
        }
        /// <summary>
        /// 登录预验证
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="account"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public bool LoginPreCheck(string ip, string account, string? captcha = null)
        {
            var today = Time.TimestampFrom(DateTime.Today);
            var count = _db.ExecuteScalar<int>($"select COUNT(id) as count from {LoginLogEntity.ND_TABLE_NAME} where ip=@0 and status=0 and created_at>=@1", ip, today);
            if (count > 20)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(account))
            {
                return true;
            }
            count = _db.ExecuteScalar<int>($"select COUNT(id) as count from {LoginLogEntity.ND_TABLE_NAME} where user=@0 and status=0 and created_at>=@1", account, today);
            if (count > 10)
            {
                return false;
            }
            if (count < 3)
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(captcha))
            {
                return false;
            }
            return true;
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
