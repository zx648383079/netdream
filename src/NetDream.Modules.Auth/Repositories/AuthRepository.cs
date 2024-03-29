﻿using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Auth.Repositories
{
    public partial class AuthRepository
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

        const string LOGIN_MODE_WEB = "web";
        const string LOGIN_MODE_APP = "app";     // APP登陆
        const string LOGIN_MODE_QR = "qr";     // 扫描登陆
        const string LOGIN_MODE_OAUTH = "oauth";  //第三方登陆

        const string UNSET_PASSWORD = "no_password";
        const string OPTION_REGISTER_CODE = "auth_register";

        private readonly IDatabase _db;
        private readonly IGlobeOption _option;
        private readonly IClientEnvironment _client;

        public AuthRepository(IDatabase db, IGlobeOption option, IClientEnvironment client)
        {
            _db = db;
            _option = option;
            _client = client;
        }

        public AuthRegisterType RegisterType {
            get {
                var val = _option.Get<int>(OPTION_REGISTER_CODE);
                return (AuthRegisterType)val;
            }
        }

        public string EmptyEmail => $"zreno_{_client.Now}@zodream.cn";

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


        public UserModel Login(LoginForm data)
        {
            if (!string.IsNullOrWhiteSpace(data.Email) && !string.IsNullOrWhiteSpace(data.Password))
            {
                return LoginEmail(data.Email, data.Password);
            }
            if (!string.IsNullOrWhiteSpace(data.Mobile))
            {
                if (!string.IsNullOrWhiteSpace(data.Password))
                {
                    return LoginMobile(data.Mobile, data.Password);
                }
                if (!string.IsNullOrWhiteSpace(data.Code))
                {
                    return LoginMobileCode(data.Mobile, data.Code);
                }
            }
            throw new ArgumentException("form error");
        }

        public UserModel LoginEmail(string email, string password)
        {
            var user = _db.Single<UserEntity>("where email=@0", email);
            if (user == null)
            {
                LogLogin(email);
                throw new ArgumentException("email is not sign in");
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                LogLogin(email, user.Id);
                throw new ArgumentException("password is error");
            }
            LogLogin(email, user.Id, true);
            return new UserModel()
            {
                Id = user.Id,
            };
        }

        public UserModel LoginMobile(string mobile, string password)
        {
            var user = _db.Single<UserEntity>("where mobile=@0", mobile);
            if (user == null)
            {
                LogLogin(mobile);
                throw new ArgumentException("mobile is not sign in");
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                LogLogin(mobile, user.Id);
                throw new ArgumentException("password is error");
            }
            LogLogin(mobile, user.Id, true);
            return new UserModel()
            {
                Id = user.Id,
            };
        }

        public UserModel LoginMobileCode(string mobile, string code)
        {
            var user = _db.Single<UserEntity>("where mobile=@0", mobile);
            if (user == null)
            {
                LogLogin(mobile);
                throw new ArgumentException("mobile is not sign in");
            }
            // TODO 验证
            LogLogin(mobile, user.Id, true);
            return new UserModel()
            {
                Id = user.Id,
            };
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
                    CreatedAt = _client.Now
                };
                _db.Insert(model);
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
                log = _db.Single<InviteCodeEntity>("where code=@0 and (expired_at>@1 or expired_at=0)", code, _client.Now);
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
                log.ExpiredAt = _client.Now - 1;
            }
            _db.Update(log, new string[] {"invite_count", "expired_at"});
            _db.Insert(new InviteLogEntity
            {
                Code = code,
                UserId = user.Id,
                ParentId = log.UserId,
                CreatedAt = _client.Now,
            });
            return user;
        }

        public bool IsExist(string value, string name = "email")
        {
            var count = _db.ExecuteScalar<int>($"select COUNT(id) as count from {UserEntity.ND_TABLE_NAME} where {name}=@0", value);
            return count > 0;
        }

        public bool IsBan(string account)
        {
            return false;
        }

        public void LogLogin(string account, int userId = 0, bool status = false)
        {
            _db.Insert(new LoginLogEntity
            {
                Ip = _client.Ip,
                UserId = userId,
                Mode = _client.ClientName,
                Status = status ? 1 : 0,
                User = account,
                CreatedAt = _client.Now
            });
        }

        [GeneratedRegex("^zreno_\\d{11}@zodream\\.cn$")]
        private static partial Regex EmptyEmailRegex();
    }
}
