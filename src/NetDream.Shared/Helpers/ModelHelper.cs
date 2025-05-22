using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Helpers
{
    public static class ModelHelper
    {
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
        /// <summary>
        /// 获取两个数组之间的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static T[] Diff<T>(IEnumerable<T> items, T[] exclude)
        {
            return [.. items.Where(i => !exclude.Contains(i)).Distinct()];
        }
        /// <summary>
        /// 获取两个数组之间的差集
        /// </summary>
        /// <param name="items"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static int[] Diff(IEnumerable<int> items, int[] exclude)
        {
            return [.. items.Where(i => i > 0 && !exclude.Contains(i)).Distinct()];
        }
    }
}
