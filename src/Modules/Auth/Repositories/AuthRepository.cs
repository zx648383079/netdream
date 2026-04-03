using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.UserAccount;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Drawing;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Repositories
{
    public partial class AuthRepository(
        UserContext userDB,
        AuthContext db, 
        IGlobeOption option, 
        IClientContext client,
        IEventBus mediator,
        IMessageProtocol message) : IContextRepository
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


        public async Task<IOperationResult<IUserProfile>> LoginAsync(ISignInForm data)
        {
            if (data is not IContextForm form)
            {
                return OperationResult.Fail<IUserProfile>("form is error");
            }
            var res = form.Verify(this);
            if (!string.IsNullOrWhiteSpace(data.Account) || res.Succeeded)
            {
                await mediator.Publish(UserSignIn.Create(client, data.Account, res.Result?.Id ?? 0, res.Succeeded));
            }
            if (res.Succeeded)
            {
                await client.LoginAsync(res.Result);
                if (res.Result is IUserToken u && !string.IsNullOrEmpty(u.Token))
                {
                    await mediator.Publish(new TokenCreated(u.Token, res.Result, 30 * 86400));
                }
            }
            return res;
        }

        public async Task<IOperationResult> LogoutAsync()
        {
            var token = await client.LogoutAsync();
            if (!string.IsNullOrEmpty(token))
            {
                await mediator.Publish(new TokenCancel(token));
            }
            return OperationResult.Ok();
        }

        public async Task<IOperationResult<IUserProfile>> RegisterAsync(ISignUpForm data)
        {
            if (data is not IContextForm form)
            {
                return OperationResult.Fail<IUserProfile>("form is error");
            }
            var res = form.Verify(this);
            if (!string.IsNullOrWhiteSpace(data.Account) || res.Succeeded)
            {
                await mediator.Publish(UserSignIn.Create(client, data.Account, res.Result?.Id ?? 0, res.Succeeded));
            }
            if (!res.Succeeded)
            {
                return res;
            }
            if (res.Result is UserEntity o && o.Status == UserRepository.STATUS_UN_CONFIRM)
            {
                var code = message.GenerateCode(16, false);
                var param = message.Encode($"{o.Email}|{code}");
                message.SendCode(o.Email, "register_email", code, new Dictionary<string, string>()
                {
                    {"name", o.Name },
                    { "url", $"./register/verify?code={param}" }
                });
            } else
            {
                await client.LoginAsync(res.Result);
                if (res.Result is IUserToken u && !string.IsNullOrEmpty(u.Token))
                {
                    await mediator.Publish(new TokenCreated(u.Token, res.Result, 30 * 86400));
                }
            }
            return res;
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
            return message.VerifyCode(target, "login_code", code, true);
        }

        public IOperationResult<QrResult> QrRefresh()
        {
            var store = new InviteRepository(db, client, null);
            var token = store.CreateNew(InviteRepository.TYPE_LOGIN, 1, 300);
            return OperationResult.Ok(new QrResult()
            {
                Token = token,
                Qr = QRCode.ToBase64String(store.LoginQr(token))
            });
        }

        public async Task<IOperationResult<UserProfileModel>> QrCheckAsync(string token)
        {
            var res = new InviteRepository(db, client, null).CheckQr(InviteRepository.TYPE_LOGIN, token);
            if (!res.Succeeded)
            {
                return OperationResult<UserProfileModel>.Fail(res);
            }
            var user = userDB.Users.Where(i => i.Id == res.Result).SingleOrDefault();
            if (user is null)
            {
                return OperationResult<UserProfileModel>.Fail("account is error");
            }
            var model = new UserProfileModel(user);
            await mediator.Publish(UserSignIn.Ok(client, "qr", model));
            await client.LoginAsync(model);
            if (!string.IsNullOrEmpty(model.Token))
            {
                await mediator.Publish(new TokenCreated(model.Token, model, 30 * 86400));
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult QrAuthorize(QrAuthorizeForm data)
        {
            return new InviteRepository(db, client, null)
                .Authorize(InviteRepository.TYPE_LOGIN, data.Token, data.Confirm, data.Reject);
        }
        public IOperationResult SendFindEmail(string email)
        {
            if (!Validator.IsEmail(email))
            {
                return OperationResult.Fail(FailureReasons.ValidateError, "email is empty");
            }
            if (IsBan(email))
            {
                return OperationResult.Fail("邮箱已列入黑名单");
            }
            var res = Find(i => i.Email == email);
            if (!res.Succeeded)
            {
                return res;
            }
            var user = res.Result;
            var code = message.GenerateCode(8);
            var param = message.Encode($"{user.Email}|{code}");
            return message.SendCode(user.Email, "find_code", code, new Dictionary<string, string>()
            {
                { "name", user.Name },
                { "time", TimeHelper.Format()},
                { "code", code },
                { "url", $"./password?code={param}" }
            });
        }

        public IOperationResult SendCode(CodeRequestForm data)
        {
            var res = Validate(data);
            if (!res.Succeeded) 
            {
                return res;
            }
            if (IsBan(data.To))
            {
                return OperationResult.Fail("已列入黑名单，禁止发送验证码");
            }
            var user = userDB.Users.When(data.ToType == "email", i => i.Email == data.To, i => i.Mobile == data.To)
                .Select(i => new UserEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).SingleOrDefault();
            if (user is not null && data.Event is "verify_new" or "register")
            {
                return OperationResult.Fail("已存在");
            }
            var code = message.GenerateCode(8);
            res = message.SendCode(data.To, "login_code", code, new Dictionary<string, string>()
            {
                { "name", user.Name },
                { "time", TimeHelper.Format()},
                { "code", code },
            });
            if (!res.Succeeded)
            {
                return res;
            }
            return OperationResult.Ok();
        }

        public IOperationResult VerifyCode(CodeVerifyForm data)
        {
            var res = Validate(data);
            if (!res.Succeeded)
            {
                return res;
            }
            if (!message.VerifyCode(data.To, "login_code", data.Code, false))
            {
                return OperationResult.Fail("验证码错误");
            }
            return OperationResult.Ok();
        }

        private IOperationResult Validate(ICodeRequestForm data)
        {
            if (!(data.ToType is "email" or "mobile") 
                || !(data.Event is "verify_old" or "verify_new" or "login" or "register"))
            {
                return OperationResult.Fail("check data error");
            }
            if (data.Event == "verify_old")
            {
                if (!client.TryGetUser(out var user))
                {
                    return OperationResult.Fail("请先登录");
                }
                if (data.ToType == "email")
                {
                    if (IsEmptyEmail(user.Email))
                    {
                        return OperationResult.Fail("email error");
                    }
                    data.To = user.Email;
                } else
                {
                    if (string.IsNullOrWhiteSpace(user.Mobile))
                    {
                        return OperationResult.Fail("未绑定手机号");
                    }
                    data.To = user.Mobile;
                }
                return OperationResult.Ok();
            }
            if (data.ToType == "email" && !Validator.IsEmail(data.To))
            {
                return OperationResult.Fail("email is error");
            }
            if (data.ToType == "mobile" && !Validator.IsMobile(data.To))
            {
                return OperationResult.Fail("mobile is error");
            }
            return OperationResult.Ok();
        }

        public IOperationResult ResetPassword(ResetPasswordForm data)
        {
            if (!Validator.IsEmail(data.Email))
            {
                return OperationResult.Fail(FailureReasons.ValidateError, "email is empty");
            }
            if (data.Password.Length < 6)
            {
                return OperationResult.Fail("密码长度必须不小于6位");
            }
            if (data.Password != data.ConfirmPassword)
            {
                return OperationResult.Fail("两次密码不一致");
            }
            if (message.VerifyCode(data.Email, "find_code", data.Code))
            {
                return OperationResult.Fail("邮箱或安全码不正确");
            }
            var user = userDB.Users.Where(i => i.Email == data.Email).FirstOrDefault();
            if (user is null || UserRepository.IsActive(user) || user.Email != data.Email)
            {
                return OperationResult.Fail("邮箱或安全码不正确");
            }
            if (BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "password is not changed");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
            userDB.Users.Save(user);
            userDB.SaveChanges();
            mediator.Publish(UserAction.Create(client, "password", "重置密码"));
            return OperationResult.Ok();
        }

        public IOperationResult UpdatePassword(UpdatePasswordForm data)
        {
            if (!string.IsNullOrEmpty(data.OldPassword))
            {
                data.VerifyType = "password";
                data.Verify = data.OldPassword;
            }
            if (string.IsNullOrEmpty(data.Verify))
            {
                return OperationResult.Fail(FailureReasons.ValidateError, " is empty");
            }
            if (data.Password.Length < 6)
            {
                return OperationResult.Fail("密码长度必须不小于6位");
            }
            if (data.Password != data.ConfirmPassword)
            {
                return OperationResult.Fail("两次密码不一致");
            }
            var user = userDB.Users.Where(i => i.Id == client.UserId).FirstOrDefault();
            if (user is null || UserRepository.IsActive(user))
            {
                return OperationResult.Fail("账户错误");
            }
            if (data.VerifyType == "password")
            {
                if (user.Password != UNSET_PASSWORD && !BCrypt.Net.BCrypt.Verify(data.Verify, user.Password))
                {
                    return OperationResult.Fail("密码不正确！");
                }
            } else if (data.VerifyType == "email")
            {
                if (!message.VerifyCode(user.Email, "login_code", data.Verify, true))
                {
                    return OperationResult.Fail("验证码不正确！");
                }
            }
            else
            {
                if (!message.VerifyCode(user.Mobile, "login_code", data.Verify, true))
                {
                    return OperationResult.Fail("验证码不正确！");
                }
            }
            if (BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "password is not changed");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
            userDB.Users.Save(user);
            userDB.SaveChanges();
            mediator.Publish(UserAction.Create(client, "password", "重置密码"));
            return OperationResult.Ok();
        }

        [GeneratedRegex("^zreno_\\d{11}@zodream\\.cn$")]
        private static partial Regex EmptyEmailRegex();

 
    }
}
