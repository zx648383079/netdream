﻿using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using System.Text.RegularExpressions;
using System;

namespace NetDream.Modules.Auth.Repositories
{
    public partial class AuthRepository(IDatabase db, IGlobeOption option, IClientContext client)
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
            var count = db.FindCount<int, LoginLogEntity>("id", "ip=@0 and status=0 and created_at>=@1", ip, today);
            if (count > 20)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(account))
            {
                return true;
            }
            count = db.FindCount<int, LoginLogEntity>("id", "user=@0 and status=0 and created_at>=@1", account, today);
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


        public IOperationResult<IUser> Login(ISignInForm data)
        {
            var res = data.Verify(db);
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
                db.Insert(model);
                return model;
            });
            return new UserModel();
        }

        public T CheckInviteCode<T>(string code, Func<int, T> func)
            where T : UserEntity
        {
            InviteCodeEntity? log = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                log = db.Single<InviteCodeEntity>("where code=@0 and (expired_at>@1 or expired_at=0)", code, client.Now);
            }
            if (log is null)
            {
                if (RegisterType == AuthRegisterType.InviteCode)
                {
                    throw new ArgumentException("必须输入邀请码");
                }
                return func.Invoke(0);
            }
            var user = func.Invoke(log.UserId);
            log.InviteCount++;
            if (log.Amount > 0 && log.InviteCount == log.Amount)
            {
                log.ExpiredAt = client.Now - 1;
            }
            db.Update(log, new string[] { "invite_count", "expired_at" });
            db.Insert(new InviteLogEntity
            {
                Code = code,
                UserId = user.Id,
                ParentId = log.UserId,
                CreatedAt = client.Now,
            });
            return user;
        }

        public bool IsExist(string value, string name = "email")
        {
            var count = db.FindCount<int, UserEntity>("id", $"where {db.DatabaseType.EscapeSqlIdentifier(name)}=@0", value);
            return count > 0;
        }

        public bool IsBan(string account)
        {
            return false;
        }

        public void LogLogin(string account, int userId = 0, bool status = false)
        {
            db.Insert(new LoginLogEntity
            {
                Ip = client.Ip,
                UserId = userId,
                Mode = client.ClientName,
                Status = status ? 1 : 0,
                User = account,
                CreatedAt = client.Now
            });
        }

        [GeneratedRegex("^zreno_\\d{11}@zodream\\.cn$")]
        private static partial Regex EmptyEmailRegex();
    }
}
