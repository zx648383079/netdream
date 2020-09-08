using Microsoft.AspNetCore.Http;
using NetDream.Areas.Open.Entities;
using NetDream.Base.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetDream.Areas.Open.Models
{
    public class PlatformModel: PlatformEntity
    {
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
                    if (verifyOneRule(rule.Trim().Substring(1), path))
                    {
                        return false;
                    }
                    continue;
                }
                if (verifyOneRule(rule.Trim(), path))
                {
                    return true;
                }
            }
            return true;
        }

        private bool verifyOneRule(string rule, string path)
        {
            if (rule == "*")
            {
                return true;
            }
            return (rule[0]) switch
            {
                '@' => true,// 验证模块，暂缺
                '^' => Regex.IsMatch(path, rule),
                '~' => Regex.IsMatch(path, rule.Substring(1)),
                _ => rule == path,
            };
        }

        public bool VerifyRest(HttpContext context)
        {
            if (SignType < 1)
            {
                return true;
            }
            if (SignType == 1)
            {
                var data = new Dictionary<string, string>();
                foreach (var item in context.Request.Query)
                {
                    if (item.Key == "sign")
                    {
                        continue;
                    }
                    data.Add(item.Key, item.Value.ToString());
                }
                return Sign(data) == context.Request.Query["sign"].ToString();
            }
            return false;
        }

        public string Sign(Dictionary<string, string> data)
        {
            if (SignType < 1)
            {
                return "";
            }
            var content = getSignContent(data);
            if (SignType == 1)
            {
                return Str.MD5Encode(content);
            }
            return "";
        }

        private string getSignContent(Dictionary<string, string> data)
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
                if (data.ContainsKey(key))
                {
                    sb.Append(data[key]);
                    continue;
                }
            }
            return sb.ToString();
        }
    }
}
