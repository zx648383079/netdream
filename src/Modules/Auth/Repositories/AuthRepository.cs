using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.UserAccount;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Auth.Repositories
{
    public partial class AuthRepository(
        UserContext userDB,
        AuthContext db, 
        IGlobeOption option, 
        IClientContext client) : IContextRepository
    {
        internal const byte ACCOUNT_TYPE_NAME = 1;
        internal const byte ACCOUNT_TYPE_EMAIL = 2;
        internal const byte ACCOUNT_TYPE_MOBILE = 3;
        internal const byte ACCOUNT_TYPE_OAUTH_QQ = 11;
        internal const byte ACCOUNT_TYPE_OAUTH_WX = 12;
        internal const byte ACCOUNT_TYPE_OAUTH_WX_MINI = 13;
        internal const byte ACCOUNT_TYPE_OAUTH_WEIBO = 14;
        internal const byte ACCOUNT_TYPE_OAUTH_TAOBAO = 15;
        internal const byte ACCOUNT_TYPE_OAUTH_ALIPAY = 16;
        internal const byte ACCOUNT_TYPE_OAUTH_GITHUB = 21;
        internal const byte ACCOUNT_TYPE_OAUTH_GOOGLE = 21;
        internal const byte ACCOUNT_TYPE_ID_CARD = 98;
        internal const byte ACCOUNT_TYPE_IP = 99;

        public const string OAUTH_TYPE_QQ = "qq";
        public const string OAUTH_TYPE_WX = "wx";
        public const string OAUTH_TYPE_WX_MINI = "wx_mini"; // 微信小程序
        public const string OAUTH_TYPE_WEIBO = "weibo";
        public const string OAUTH_TYPE_TAOBAO = "taobao";
        public const string OAUTH_TYPE_ALIPAY = "alipay";
        public const string OAUTH_TYPE_WEBAUTHN = "web_authn";
        public const string OAUTH_TYPE_GITHUB = "github";
        public const string OAUTH_TYPE_GOOGLE = "google";
        public const string OAUTH_TYPE_2FA = "2fa";

        public const string LOGIN_MODE_WEB = "web";
        public const string LOGIN_MODE_APP = "app";     // APP登陆
        public const string LOGIN_MODE_QR = "qr";     // 扫描登陆
        public const string LOGIN_MODE_OAUTH = "oauth";  //第三方登陆

        const string UNSET_PASSWORD = "no_password";
        public const string OPTION_REGISTER_CODE = "auth_register";
        public const string OPTION_OAUTH_CODE = "auth_oauth";

        public AuthRegisterType RegisterType {
            get {
                var val = option.Get<int>(OPTION_REGISTER_CODE);
                return (AuthRegisterType)val;
            }
        }

        public string EmptyEmail => $"zreno_{client.Now}@zodream.cn";

        public static bool IsEmptyEmail(string? email)
        {
            return string.IsNullOrWhiteSpace(email) || EmptyEmailRegex().IsMatch(email);
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
            var today = TimeHelper.TimestampFrom(DateTime.Today);
            var count = db.LoginLogs.Where(i => i.Ip == ip && i.Status == 0 && i.CreatedAt>= today)
               .Count();
            if (count > 20)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(account))
            {
                return true;
            }
            count = db.LoginLogs.Where(i => i.User == account && i.Status == 0 && i.CreatedAt >= today).Count();
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


        public IOperationResult<IUserProfile> Login(ISignInForm data)
        {
            if (data is not IContextForm form)
            {
                return OperationResult.Fail<IUserProfile>("form is error");
            }
            var res = form.Verify(this);
            if (!string.IsNullOrWhiteSpace(data.Account) || res.Succeeded)
            {
                LogLogin(data.Account, res.Result?.Id ?? 0, res.Succeeded);
            }
            return res;
        }

        public UserModel Register(RegisterForm data)
        {
            if (!data.Agreement)
            {
                throw new ArgumentException("必须同意相关协议");
            }
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                throw new ArgumentException("请输入昵称");
            }
            if (string.IsNullOrWhiteSpace(data.Mobile))
            {
                if (string.IsNullOrWhiteSpace(data.Email))
                {
                    throw new ArgumentException("请输入Email");
                }
                if (IsBan(data.Email))
                {
                    throw new ArgumentException("此Email禁止注册");
                }
                if (IsExist(data.Email))
                {
                    throw new ArgumentException("此Email已注册");
                }
                if (string.IsNullOrWhiteSpace(data.Password) || data.Password != data.ConfirmPassword)
                {
                    throw new ArgumentException("请输入 Password 并确认密码");
                }
            } else
            {
                // TODO 验证验证码
            }
            var user = CheckInviteCode(data.InviteCode, parentId => {
                var model = new UserEntity
                {
                    Name = data.Name,
                    Mobile = data.Mobile,
                    Email = string.IsNullOrWhiteSpace(data.Email) ? EmptyEmail : data.Email,
                    ParentId = parentId,
                    Password = string.IsNullOrWhiteSpace(data.Password) ? UNSET_PASSWORD 
                    : BCrypt.Net.BCrypt.HashPassword(data.Password),
                    CreatedAt = client.Now
                };
                userDB.Users.Add(model);
                db.SaveChanges();
                return model;
            });
            return new UserModel();
        }

        public IOperationResult<UserEntity> CheckInviteCode(string code, Func<int, UserEntity> func)
        {
            InviteCodeEntity? log = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                log = db.InviteCodes.Where(i => i.Token == code && i.Type == InviteRepository.TYPE_CODE)
                    .Where(i => i.ExpiredAt > client.Now || i.ExpiredAt == 0)
                    .Single();
            }
            if (log is null)
            {
                if (RegisterType == AuthRegisterType.InviteCode)
                {
                    return OperationResult.Fail<UserEntity>("必须输入邀请码");
                }
                return OperationResult.Ok(func.Invoke(0));
            }
            var user = func.Invoke(log.UserId);
            log.InviteCount++;
            if (log.Amount > 0 && log.InviteCount == log.Amount)
            {
                log.ExpiredAt = client.Now - 1;
            }
            db.InviteCodes.Update(log);
            db.InviteLogs.Add(new InviteLogEntity
            {
                CodeId = log.Id,
                UserId = user.Id,
                ParentId = log.UserId,
                CreatedAt = client.Now,
            });
            db.SaveChanges();
            return OperationResult.Ok(user);
        }

        public bool IsExist(string value, string name = "email")
        {
            return userDB.Users.Where(name, value).Any();
        }

        public bool IsBan(string account)
        {
            return db.BanAccounts.Where(i => i.ItemKey == account).Any();
        }

        public void LogLogin(string account, int userId = 0, bool status = false)
        {
            db.LoginLogs.Add(new LoginLogEntity
            {
                Ip = client.Ip,
                UserId = userId,
                Mode = client.ClientName,
                Status = status ? 1 : 0,
                User = account,
                CreatedAt = client.Now
            });
            db.SaveChanges();
        }

        public static string GetLastIp(AuthContext db, int user)
        {
            var ip = db.LoginLogs
                .Where(i => i.UserId == user && i.Status == 1)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.Ip)
                .Take(1)
                .SingleOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                return string.Empty;
            }
            return StrHelper.HideIp(ip);
        }

        public IOperationResult<IUserProfile> Create(UserEntity user, string inviteCode = "")
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                user.Name = $"zre_{client.Now}";
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = EmptyEmail;
                user.Status = UserRepository.STATUS_ACTIVE;
            } 
            else
            {
                user.Status = UserRepository.STATUS_UN_CONFIRM;
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = UNSET_PASSWORD;
            }
            var res = CheckInviteCode(inviteCode, parentId => {
                userDB.Users.Add(user);
                userDB.SaveChanges();
                return user;
            });
            if (!res.Succeeded)
            {
                return OperationResult<IUserProfile>.Fail(res);
            }
            return OperationResult.Ok<IUserProfile>(user);
        }

        public bool Exists(Expression<Func<UserEntity, bool>> where)
        {
            return userDB.Users.Where(where).Any();
        }

        public IOperationResult<IUserProfile> Find(Expression<Func<UserEntity, bool>> where)
        {
            var model = userDB.Users.Where(where).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<IUserProfile>("account is error");
            }
            return OperationResult.Ok<IUserProfile>(new UserProfileModel(model));
        }

        public IOperationResult<IUserProfile> Find(Expression<Func<UserEntity, bool>> where, string password)
        {
            var model = userDB.Users.Where(where).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<IUserProfile>("account is error");
            }
            if (!BCrypt.Net.BCrypt.Verify(password, model.Password))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "password is error");
            }
            return OperationResult.Ok<IUserProfile>(new UserProfileModel(model));
        }

        public bool VerifyCode(string target, string code)
        {
            throw new NotImplementedException();
        }

        [GeneratedRegex("^zreno_\\d{11}@zodream\\.cn$")]
        private static partial Regex EmptyEmailRegex();
    }
}
