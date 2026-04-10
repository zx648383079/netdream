using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class MetaRepository(ArticleContext db): IMetaRepository
    {

        internal static Dictionary<string, string> ArticleDefaultItems => new()
        {
            { "is_hide", "0" }, // 如果是转载文章是否只显示部分，并链接到原文
            { "source_url", string.Empty }, // 原文链接
            {"source_author", string.Empty }, // 原文作者
            { "cc_license", string.Empty }, // 版权协议
            { "weather", string.Empty }, // 天气
            { "audio_url", string.Empty }, // 音频
            { "video_url", string.Empty }, // 视频
            { "comment_status", "0" }, // 是否允许评论
            { "seo_link", string.Empty }, // 优雅链接
            { "seo_title", string.Empty }, // "seo 优化标题",
            { "seo_description", string.Empty }, // "seo 优化描述",
        };



        public IDictionary<string, string> Get(ModuleTargetType type, int target, string language)
        {
            return db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target && i.Language == language)
                .Select(i => new KeyValuePair<string, string>(i.Name, i.Content))
                .ToDictionary();
        }

        public IDictionary<string, string> Get(ModuleTargetType type, int target, string language, IDictionary<string, string> def)
        {
            if (target < 1)
            {
                return def;
            }
            var keys = def.Keys;
            var items = db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target && i.Language == language && keys.Contains(i.Name))
                .Select(i => new KeyValuePair<string, string>(i.Name, i.Content))
                .ToDictionary();
            foreach (var item in def)
            {
                items.TryAdd(item.Key, item.Value);
            }
            return items;
        }

        public IOperationResult Replace(ModuleTargetType type, int target, string language, IDictionary<string, string> data)
        {
            if (data.Count == 0)
            {
                db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target && i.Language == language).ExecuteDelete();
                db.SaveChanges();
                return OperationResult.Ok();
            }
            var items = db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target
            && i.Language == language).ToDictionary(i => i.Name);
            var delKeys = items.Select(i => i.Key).Where(i => !data.ContainsKey(i)).ToList();
            foreach (var item in data)
            {
                if (items.TryGetValue(item.Key, out var entity))
                {
                    if (item.Value == entity.Content)
                    {
                        continue;
                    }
                    db.Metas.Where(i => i.Id == entity.Id).ExecuteUpdate(setters => setters.SetProperty(i => i.Content, item.Value));
                    continue;
                }
                if (delKeys.Count > 0)
                {
                    entity = items[delKeys.First()];
                    delKeys.RemoveAt(0);
                    db.Metas.Where(i => i.Id == entity.Id)
                        .ExecuteUpdate(setters => setters.SetProperty(i => i.Content, item.Value)
                        .SetProperty(i => i.Name, item.Key));
                    continue;
                }
                db.Metas.Add(new()
                {
                    ItemId = target,
                    ItemType = (byte)type,
                    Language = language,
                    Name = item.Key,
                    Content = item.Value
                });
            }
            foreach (var item in delKeys)
            {
                db.Metas.Remove(items[item]);
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Update(ModuleTargetType type, int target, string language, IDictionary<string, string> data)
        {
            if (data.Count == 0)
            {
                return OperationResult.Ok();
            }
            var metaKeys = data.Keys;
            var items = db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target 
            && i.Language == language && metaKeys.Contains(i.Name)).ToDictionary(i => i.Name);
            foreach (var item in data)
            {
                if (!items.TryGetValue(item.Key, out var entity))
                {
                    db.Metas.Add(new()
                    {
                        ItemId = target,
                        ItemType = (byte)type,
                        Language = language,
                        Name = item.Key,
                        Content = item.Value
                    });
                    continue;
                }
                if (item.Value == entity.Content)
                {
                    continue;
                }
                db.Metas.Where(i => i.Id == entity.Id).ExecuteUpdate(setters => setters.SetProperty(i => i.Content, item.Value));
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Update(ModuleTargetType type, int target, string language, string name, string value)
        {
            var model = db.Metas.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.Language == language && i.Name == name).SingleOrDefault();
            if (model == null) 
            {
                db.Metas.Add(new()
                {
                    ItemId = target,
                    ItemType = (byte)type,
                    Language = language,
                    Name = name,
                    Content = value
                });
            } else
            {
                model.Content = value;
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public void Remove(ModuleTargetType type, int target, string language)
        {
            db.Metas.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.Language == language).ExecuteDelete();
            db.SaveChanges();
        }
        public void Remove(ModuleTargetType type, int target, string language, string name)
        {
            db.Metas.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.Language == language && i.Name == name).ExecuteDelete();
            db.SaveChanges();
        }

        public void Remove(ModuleTargetType type, int target)
        {
            db.Metas.Where(i => i.ItemId == target && i.ItemType == (byte)type).ExecuteDelete();
            db.SaveChanges();
        }

        public string Get(ModuleTargetType type, int target, string language, string name)
        {
            return db.Metas.Where(i => i.ItemType == (byte)type && i.ItemId == target && i.Language == language && i.Name == name)
                .Value(i => i.Content) ?? string.Empty;
        }
    }
}
