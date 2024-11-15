using NetDream.Shared.Extensions;
using NPoco;
using System.Collections.Generic;
using System.Text.Json;

namespace NetDream.Shared.Repositories
{
    public abstract class MetaRepository<T>(IDatabase db) where T : class
    {

        protected virtual string IdKey => "item_id";
        protected virtual Dictionary<string, object> DefaultItems => [];

        /**
         * 获取并合并默认的
         * @param int id
         * @return array
         */
        public Dictionary<string, object> GetOrDefault(int id)
        {
            return GetMap(id, DefaultItems);
        }

        /**
         * 获取
         * @param int id
         * @param array default
         * @return array
         */
        public Dictionary<string, object> GetMap(int id)
        {
            return GetMap(id, []);
        }

        public Dictionary<string, object> GetMap(int id, Dictionary<string, object> def)
        {
            if (id < 1)
            {
                return def;
            }
            var items = db.Dictionary<string, object>(new Sql().Select("name", "content").From<T>(db)
                .Where($"{IdKey}=@0", id));
            foreach (var item in def)
            {
                items.TryAdd(item.Key, item.Value);
            }
            return items;
        }

        public void SaveBatch(int id, Dictionary<string, object> data)
        {
            SaveBatch(id, data, []);
        }
        /**
         * 批量保存
         * @param int id
         * @param array data
         */
        public void SaveBatch(int id, Dictionary<string, object> data, Dictionary<string, object> defItems)
        {
            if (data.Count == 0)
            {
                return;
            }
            var metaKeys = defItems.Count == 0 ? DefaultItems.Keys : defItems.Keys;
            var items = GetMap(id);
            var add = new List<Dictionary<string,object>>();
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
                        {IdKey, id},
                        {"name", item.Key },
                        {"content", content }
                    });
                    continue;
                }
                if (content == items[item.Key])
                {
                    continue;
                }
                db.UpdateWhere<T>("content=@0", $"{IdKey}=@1 AND name=@2", content!, id, item.Key);
            }
            if (add.Count == 0)
            {
                return;
            }
            db.InsertBatch<T>(add);
        }

        /// <summary>
        /// 根据主id删除meta
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBatch(int id)
        {
            db.Delete<T>($"WHERE {IdKey}=@0", id);
        }
    }
}
