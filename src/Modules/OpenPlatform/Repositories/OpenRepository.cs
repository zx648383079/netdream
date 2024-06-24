using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class OpenRepository(IDatabase db, IClientEnvironment environment)
    {
        public PlatformModel GetByAppId(string appId)
        {
            return db.Single<PlatformModel>("where appid=@0", appId);
        }

        public void GenerateNewId(PlatformEntity entity)
        {
            entity.Appid = "1" + environment.Now;
            entity.Secret = StrHelper.MD5Encode(Guid.NewGuid().ToString());
        }
        /// <summary>
        /// 前台保存应用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PlatformEntity SavePlatform(PlatformForm data)
        {
            PlatformEntity model;
            if (data.Id > 0)
            {
                model = db.Single<PlatformEntity>("WHERE id=@0 AND user_id=@1", data.Id, environment.UserId);
                if (model is null)
                {
                    throw new Exception("应用不存在");
                }
            } else
            {
                model = new PlatformEntity()
                {
                    UserId = environment.UserId,
                    Status = PlatformRepository.STATUS_WAITING;
                };
                GenerateNewId(model);
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Domain = data.Domain;
            model.SignType = data.SignType;
            model.SignKey = data.SignKey;
            model.EncryptType = data.EncryptType;
            model.PublicKey = data.PublicKey;
            model.AllowSelf = data.AllowSelf;
            if (model.UpdatedAt == 0)
            {
                model.UpdatedAt = environment.Now;
            }
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = environment.Now;
            }
            db.Save(model);
            return model;
        }

        /**
         * 创建token
         * @param int platform_id
         * @return UserTokenModel
         * @throws Exception
         */
        public UserTokenEntity CreateToken(int platform_id, 
            string expired_at = "")
        {
            if (platform_id < 0)
            {
                throw new Exception("请选择应用");
            }
            var platform = db.Single<PlatformEntity>("WHERE id=@0 AND allow_self=1 AND status=1");
            if (platform is null)
            {
                throw new Exception("请选择应用");
            }
            var model = new UserTokenEntity()
            {
                UserId = environment.UserId,
                PlatformId = platform_id,
                Token = StrHelper.MD5Encode($"{environment.UserId}:{TimeHelper.Millisecond()}"),
                ExpiredAt = string.IsNullOrWhiteSpace(expired_at) ? environment.Now + 86400 : 
                    TimeHelper.TimestampFrom(expired_at),
                CreatedAt = environment.Now,
                UpdatedAt = environment.Now,
            };
            model.Id = (int)db.Insert(model);
            if (model.Id == 0)
            {
                throw new Exception("生成失败");
            }
            return model;
        }


        /// <summary>
        /// 分享接口验证网址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PlatformModel CheckUrl(string appId, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("网址不能为空");
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new Exception("应用无效");
            }
            var model = GetByAppId(appId);
            if (model is null || string.IsNullOrWhiteSpace(model.Domain))
            {
                throw new Exception("应用无效");
            }
            if (model.Domain == "*")
            {
                return model;
            }
            var host = new Uri(url).Host;
            if (host != model.Domain)
            {
                throw new Exception("应用域名不匹配");
            }
            return model;
        }

    }
}
