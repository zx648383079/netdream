using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Context;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Repositories
{
    public abstract class MetaRepository(IMetaContext db)
    {
        protected virtual Dictionary<string, string> DefaultItems => [];

        /// <summary>
        /// 获取并合并默认的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetOrDefault(int id)
        {
            return GetMap(id, DefaultItems);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetMap(int id)
        {
            return db.Metas.Where(i => i.ItemId == id)
                .Select(i => new KeyValuePair<string, string>(i.Name, i.Content))
                .ToDictionary();
        }

        /// <summary>
        /// 获取指定的
        /// </summary>
        /// <param name="id"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetMap(int id, IDictionary<string, string> def)
        {
            if (id < 1)
            {
                return def;
            }
            var keys = def.Keys;
            var items = db.Metas.Where(i => i.ItemId == id && keys.Contains(i.Name))
                .Select(i => new KeyValuePair<string, string>(i.Name, i.Content))
                .ToDictionary();
            foreach (var item in def)
            {
                items.TryAdd(item.Key, item.Value);
            }
            return items;
        }

        public void SaveBatch(int id, IDictionary<string, string> data)
        {
            SaveBatch(id, data, new Dictionary<string, string>());
        }
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="defItems"></param>
        public void SaveBatch(int id, IDictionary<string, string> data, IDictionary<string, string> defItems)
        {
            if (data.Count == 0)
            {
                return;
            }
            var metaKeys = defItems.Count == 0 ? DefaultItems.Keys : defItems.Keys;
            var items = GetMap(id);
            foreach (var item in data)
            {
                if (!metaKeys.Contains(item.Key))
                {
                    continue;
                }
                var content = item.Value;
                if (item.Value is null)
                {
                    content = string.Empty;
                }
                else if(item.Value.GetType().IsClass) {
                    content = JsonSerializer.Serialize(item.Value);
                }
                if (!items.ContainsKey(item.Key))
                {
                    if (content is null)
                    {
                        continue;
                    }
                    db.Metas.Add(new()
                    {
                        ItemId = id,
                        Name = item.Key,
                        Content = content
                    });
                    continue;
                }
                if (content == items[item.Key])
                {
                    continue;
                }
                db.Metas.Where(i => i.ItemId == id && i.Name == item.Key).ExecuteUpdate(setters => setters.SetProperty(i => i.Content, content));
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 根据主id删除 meta
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBatch(int id)
        {
            db.Metas.Where(i => i.ItemId == id).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
