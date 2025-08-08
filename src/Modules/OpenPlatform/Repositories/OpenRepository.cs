using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
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

        public ListLabelItem[] GetByUser(int user)
        {
            return db.Platforms.Where(i => i.UserId == user)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public IDictionary<string, IDictionary<string, string>> OptionGet(int user, int platform, IDictionary<string, IDictionary<string, string>> data)
        {
            if (data.Count == 0)
            {
                return data;
            }
            if (!db.Platforms.Where(i => i.UserId == user && i.Id == platform).Any())
            {
                return data;
            }
            var keys = data.Keys.ToArray();
            var items = db.PlatformOptions.Where(i => i.PlatformId == platform && keys.Contains(i.Store)).ToArray();
            foreach ( var item in items)
            {
                if (!data.TryGetValue(item.Store, out var package))
                {
                    continue;
                }
                if (!package.ContainsKey(item.Name))
                {
                    continue;
                }
                package[item.Name] = item.Value;
            }
            return data;
        }

        public IOperationResult OptionSave(int user, int platform, IDictionary<string, IDictionary<string, string>> data)
        {
            if (data.Count == 0)
            {
                return OperationResult.Fail("数据错误");
            }
            if (!db.Platforms.Where(i => i.UserId == user && i.Id == platform).Any())
            {
                return OperationResult.Fail("数据错误");
            }
            var keys = data.Keys.ToArray();
            var items = db.PlatformOptions.Where(i => i.PlatformId == platform && keys.Contains(i.Store))
                .Select(i => new PlatformOptionEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Store = i.Store,
                })
                .ToArray()
                .Select(i => new KeyValuePair<string, int>($"{i.Store}_{i.Name}", i.Id))
                .ToDictionary();
            var now = TimeHelper.TimestampNow();
            foreach (var group in data)
            {
                if (string.IsNullOrWhiteSpace(group.Key))
                {
                    continue;
                }
                foreach (var item in group.Value)
                {
                    if (item.Key.StartsWith('_') || string.IsNullOrWhiteSpace(item.Key))
                    {
                        continue;
                    }
                    var key = $"{group.Key}_{item.Key}";
                    if (items.TryGetValue(key, out var id))
                    {
                        db.PlatformOptions.Where(i => i.Id == id)
                            .ExecuteUpdate(setters => setters.SetProperty(i => i.Value, item.Value)
                            .SetProperty(i => i.UpdatedAt, now));
                        continue;
                    }
                    db.PlatformOptions.Add(new PlatformOptionEntity()
                    {
                        Store = group.Key,
                        Name = item.Key,
                        Value = item.Value,
                        PlatformId = platform,
                        CreatedAt = now,
                        UpdatedAt = now,
                    });
                }
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
