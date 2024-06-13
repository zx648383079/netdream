using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Core.Helpers
{
    public static class ModelHelper
    {
        public static string TableName<T>() where T : class
        {
            return TableName(typeof(T));
        }
        public static string TableName(Type t)
        {
            var a = t.GetType().GetCustomAttributes(typeof(TableNameAttribute), true).ToArray();
            return a.Length == 0 ? t.Name : (a[0] as TableNameAttribute)!.Value;
        }
        /// <summary>
        /// 从值获取数字数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<int> ParseArrInt(string str)
        {
            if (str.Contains('[')) 
            {
                return JsonSerializer.Deserialize<IList<int>>(str) ?? [];
            }
            if (str.Contains('{'))
            {
                var res = JsonSerializer.Deserialize<IDictionary<string, int>>(str);
                if (res is null)
                {
                    return [];
                }
                return [.. res.Values];
            }
            return ParseArrInt(str.Split(','));
        }
        /// <summary>
        /// 从值获取数字数组
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<int> ParseArrInt(IEnumerable<object> items)
        {
            return items.Select(i => {
                if (i is int o)
                {
                    return o;
                }
                if (int.TryParse(i.ToString(), out var res)) 
                {
                    return res;
                }
                return 0;
            }).Where(i => i > 0).ToArray();
        }
        /// <summary>
        /// 拆分两个数组
        /// </summary>
        /// <param name="current"></param>
        /// <param name="exist"></param>
        /// <param name="intersect">是否要真的获取共有的</param>
        /// <param name="removeEmpty">是否移除空的项</param>
        /// <returns>返回 [新增, 共有, 删除]</returns>
        public static (IList<int>, IList<int>, IList<int>) SplitId(
            IList<int> current, IList<int> exist, bool removeEmpty = true)
        {
            if (removeEmpty)
            {
                current = current.Where(i => i > 0).Distinct().ToArray();
                exist = exist.Where(i => i > 0).Distinct().ToArray();
            }
            if (!exist.Any() && !current.Any())
            {
                return ([], [], []);
            }
            if (!exist.Any())
            {
                return (current, [], []);
            }
            if (!current.Any())
            {
                return ([], [], exist);
            }
            var add = new List<int>();
            var same = new List<int>();
            var remove = new List<int>();
            foreach (var i in current) 
            {
                if (exist.Contains(i))
                {
                    same.Add(i);
                }
                else
                {
                    add.Add(i);
                }
            }
            foreach (var i in exist)
            {
                if (same.Contains(i))
                {
                    continue;
                }
                remove.Add(i);
            }
            return (add, same, remove);
        }
    }
}
