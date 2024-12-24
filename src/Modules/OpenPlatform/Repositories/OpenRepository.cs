using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class OpenRepository(OpenContext db, IClientContext environment)
    {
        public PlatformModel GetByAppId(string appId)
        {
            return db.Platforms.Where(i => i.Appid == appId).Single().CopyTo<PlatformModel>();
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
        public IOperationResult<PlatformEntity> SavePlatform(PlatformForm data)
        {
            PlatformEntity? model;
            if (data.Id > 0)
            {
                model = db.Platforms.Where(i => i.Id == data.Id && i.UserId == environment.UserId).Single();
                if (model is null)
                {
                   return OperationResult<PlatformEntity>.Fail("应用不存在");
                }
            } else
            {
                model = new PlatformEntity()
                {
                    UserId = environment.UserId,
                    Status = PlatformRepository.STATUS_WAITING,
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
            db.Platforms.Save(model, environment.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 创建 token
        /// </summary>
        /// <param name="platform_id"></param>
        /// <param name="expired_at"></param>
        /// <returns></returns>
        public IOperationResult<UserTokenEntity> CreateToken(int platform_id, 
            string expired_at = "")
        {
            if (platform_id < 0)
            {
                return OperationResult<UserTokenEntity>.Fail("请选择应用");
            }
            var platform = db.Platforms.Where(i => i.Id == platform_id && i.AllowSelf == 1 && i.Status == 1).Single();
            if (platform is null)
            {
                return OperationResult<UserTokenEntity>.Fail("请选择应用");
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
            db.UserTokens.Add(model);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult<UserTokenEntity>.Fail("生成失败");
            }
            return OperationResult.Ok(model);
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
