using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class OpenRepository(OpenContext db)
    {
        public PlatformModel GetByAppId(string appId)
        {
            return db.Platforms.Where(i => i.Appid == appId).Single().CopyTo<PlatformModel>();
        }

        /// <summary>
        /// 分享接口验证网址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IOperationResult<PlatformModel> CheckUrl(string appId, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return OperationResult<PlatformModel>.Fail("网址不能为空");
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                return OperationResult<PlatformModel>.Fail("应用无效");
            }
            var model = GetByAppId(appId);
            if (model is null || string.IsNullOrWhiteSpace(model.Domain))
            {
                return OperationResult<PlatformModel>.Fail("应用无效");
            }
            if (model.Domain == "*")
            {
                return OperationResult.Ok(model);
            }
            var host = new Uri(url).Host;
            if (host != model.Domain)
            {
                return OperationResult<PlatformModel>.Fail("应用域名不匹配");
            }
            return OperationResult.Ok(model);
        }

    }
}
