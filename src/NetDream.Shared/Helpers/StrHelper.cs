using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NetDream.Shared.Helpers
{
    public static class StrHelper
    {

        public static string Studly(string val)
        {
            var data = val.Split('-', '_', ' ');
            var res = new StringBuilder();
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                res.Append(item[..1].ToUpper());
                res.Append(item[1..].ToLower());
            }
            return res.ToString();
        }

        public static string Base64Encode(string val)
        {
            var bytes = Encoding.UTF8.GetBytes(val);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string val)
        {
            var bytes = Convert.FromBase64String(val);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string MD5Encode(string source)
        {
            var sor = Encoding.UTF8.GetBytes(source);
            var result = MD5.HashData(sor);
            return Convert.ToHexString(result).ToLower();
        }

        public static string Repeat(string text, int count)
        {
            return string.Concat(Enumerable.Repeat(text, count));
        }

        public static string Repeat(char text, int count)
        {
            return new string(text, count);
        }

        /// <summary>
        /// 生成数字组成的随机字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumber(int length)
        {
            var rand = new Random();
            var code = string.Empty;
            for (var i = 0; i < length; i++)
            {
                code += rand.Next(0, 10);
            }
            return code;
        }

        /// <summary>
        /// 生成大小写数字组成的随机字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Random(int length)
        {
            var rand = new Random();
            var code = string.Empty;
            for (var i = 0; i < length; i++)
            {
                code += Letter(rand.Next(0, 62));
            }
            return code;
        }

        private static char Letter(int index)
        {
            if (index < 10)
            {
                return Convert.ToChar(48 + index);
            }
            if (index < 36)
            {
                return Convert.ToChar(87 + index);
            }
            if (index < 62)
            {
                return Convert.ToChar(29 + index);
            }
            return '-';
        }

        /// <summary>
        /// 隐藏姓名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string HideName(string name)
        {
            return HideText(name, 1, 100, 1);
        }

        /// <summary>
        /// 隐藏电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string HideTel(string phone)
        {
            return HideText(phone, 1, 5, 3);
        }

        /// <summary>
        /// 隐藏邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string HideEmail(string email)
        {
            var index = email.IndexOf('@');
            if (index < 0)
            {
                return HideText(email, 1, 5, 1);
            }
            var first = Math.Min(4, (int)Math.Ceiling((double)index / 2));
            var middle = Math.Min(3, (int)Math.Floor((double)index / 2));
            return string.Format("{0}{1}{2}", email[0..first], Repeat('*', middle),
                email[index..]);
        }

        public static string HideIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return string.Empty;
            }
            var tag = ip.Contains(':') ? ':' : '.';
            var items = ip.Split(tag);
            var last = items.Length - 1;
            if (last < 3)
            {
                return ip;
            }
            for (var i = 0; i <= last; i++)
            {
                if (i == 0 || i == last)
                {
                    continue;
                }
                items[i] = "*";
            }
            return string.Join(tag, items);
        }

        public static string HideText(string text, int first = 1, int middle = 2, int end = 1)
        {
            var len = text.Length;
            if (len < 2)
            {
                return text;
            }
            if (len <= first + end)
            {
                first = 0;
                end = 1;
                middle = len - end - first;
            }
            else if(len <= middle + end) {
                middle = len - end - first;
            } else
            {
                first = len - end - middle;
            }
            return string.Format("{0}{1}{2}", text[0..first], 
                Repeat('*', middle),
                text[(first + middle)..]);
        }
    }
}
