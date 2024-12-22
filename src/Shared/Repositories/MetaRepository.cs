using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Repositories
{
    public abstract class MetaRepository(IMetaContext db)
    {
        protected virtual Dictionary<string, string> DefaultItems => [];

        /**
         * 获取并合并默认的
         * @param int id
         * @return array
         */
        public Dictionary<string, string> GetOrDefault(int id)
        {
            return GetMap(id, DefaultItems);
        }

        /**
         * 获取
         * @param int id
         * @param array default
         * @return array
         */
        public Dictionary<string, string> GetMap(int id)
        {
            return GetMap(id, []);
        }

        public Dictionary<string, string> GetMap(int id, Dictionary<string, string> def)
        {
            if (id < 1)
            {
                return def;
            }
            var items = db.Metas.Where(i => i.ItemId == id)
                .ToDictionary(i => i.Name, i => i.Content);
            foreach (var item in def)
            {
                items.TryAdd(item.Key, item.Value);
            }
            return items;
        }

        public void SaveBatch(int id, Dictionary<string, string> data)
        {
            SaveBatch(id, data, []);
        }
        /**
         * 批量保存
         * @param int id
         * @param array data
         */
        public void SaveBatch(int id, Dictionary<string, string> data, Dictionary<string, string> defItems)
        {
            if (data.Count == 0)
            {
                return;
            }
            var metaKeys = defItems.Count == 0 ? DefaultItems.Keys : defItems.Keys;
            var items = GetMap(id);
            var add = new List<MetaEntity>();
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
                    add.Add(new()
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
            if (add.Count == 0)
            {
                return;
            }
            db.Metas.AddRange(add);
            db.SaveChanges();
        }

        /// <summary>
        /// 根据主id删除 meta
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBatch(int id)
        {
            db.Metas.Where(i => i.ItemId == id).ExecuteDelete();
        }
    }
}
