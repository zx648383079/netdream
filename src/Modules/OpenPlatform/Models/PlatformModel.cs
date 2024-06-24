using NetDream.Shared.Helpers;
using NetDream.Modules.OpenPlatform.Entities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace NetDream.Modules.OpenPlatform.Models
{
    public class PlatformModel : PlatformEntity
    {

        public int RequestTime()
        {
            return 0;
        }

        public int ResponseTime()
        {
            return TimeHelper.TimestampFrom(DateTime.Now);
        }

        /// <summary>
        /// 根据路径判断是否允许访问此路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool VerifyRule(string uri)
        {
            var path = uri.Replace("/open/", "").ToLower();
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }
            foreach (var rule in Rules.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(rule))
                {
                    continue;
                }
                if (rule.StartsWith('-'))
                {
                    if (VerifyOneRule(rule.Trim()[1..], path))
                    {
                        return false;
                    }
                    continue;
                }
                if (VerifyOneRule(rule.Trim(), path))
                {
                    return true;
                }
            }
            return true;
        }

        private static bool VerifyOneRule(string rule, string path)
        {
            if (rule == "*")
            {
                return true;
            }
            return (rule[0]) switch
            {
                '@' => true,// 验证模块，暂缺
                '^' => Regex.IsMatch(path, rule),
                '~' => Regex.IsMatch(path, rule[1..]),
                _ => rule == path,
            };
        }

        public string Sign(Dictionary<string, string> data)
        {
            if (SignType < 1)
            {
                return "";
            }
            var content = GetSignContent(data);
            if (SignType == 1)
            {
                return StrHelper.MD5Encode(content);
            }
            return "";
        }

        private string GetSignContent(Dictionary<string, string> data)
        {
            data.TryAdd("appid", Appid);
            data.TryAdd("secret", Secret);
            var sb = new StringBuilder();
            if (SignKey.IndexOf('+') < 0)
            {
                var keys = data.Select(item => item.Key).OrderBy(item => item).ToArray();
                foreach (var key in keys)
                {
                    sb.Append(data[key]);
                }
                sb.Append(SignKey);
                return sb.ToString();
            }
            foreach (var key in SignKey.Split('+'))
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    sb.Append('+');
                    continue;
                }
                if (data.TryGetValue(key, out var value))
                {
                    sb.Append(value);
                    continue;
                }
            }
            return sb.ToString();
        }
    }
}
