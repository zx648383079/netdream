using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Repositories
{
    public class FileRepository
    {

        /// <summary>
        /// 根据拓展名判断是否是类型
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeExtension(string extension, string type)
        {
            if (extension.StartsWith('.'))
            {
                extension = extension[1..];
            }
            var items = TypeExtension(type);
            if (string.IsNullOrWhiteSpace(items))
            {
                return false;
            }
            return items.Split('|').Contains(extension.ToLower());
        }

        /// <summary>
        /// 根据文件类型获取拓展名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TypeExtension(string type)
        {
            var maps = new Dictionary<string, string>()
            {

            };
            if (maps.TryGetValue(type, out var res))
            {
                return res;
            }
            return string.Empty;
        }
    }
}
