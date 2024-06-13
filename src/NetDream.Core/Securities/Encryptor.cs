using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NetDream.Core.Securities
{
    public class Encryptor : ISecurity
    {

        public string Encrypt(string data)
        {
            var keyItems = CreateKeys(DateTime.Now);
            var sb = new StringBuilder();
            var buff = Encoding.UTF8.GetBytes(data);
            var bs = Convert.ToBase64String(buff);
            for (int i = 0; i < bs.Length; i++)
            {
                sb.Append(DictionaryCode(bs[i] - keyItems[i % keyItems.Length]));
            }
            return sb.ToString();
        }

        public string Decrypt(string data)
        {
            var keyItems = CreateKeys(DateTime.Now);
            var i = 0;
            var j = 0;
            var sb = new StringBuilder();
            while (i < data.Length)
            {
                var step = 1;
                if (!IsDictionaryCode(data[i]))
                {
                    step++;
                }
                sb.Append(Convert.ToChar(DictionaryKey(
                    data.Substring(i, i + step) + keyItems[j % keyItems.Length])));
                i += step;
                j++;
            }
            var buff = Convert.FromBase64String(sb.ToString());
            return Encoding.UTF8.GetString(buff);
        }

        private int[] CreateKeys(string date) 
        {
            if (Regex.IsMatch(date, @"^\d{10,}$"))
            {
                return CreateKeysFromTimestamp(date);
            }
            if (DateTime.TryParse(date, out var d))
            {
                return CreateKeys(d);
            }
            return CreateKeys(DateTime.Now);
        }

        private int[] CreateKeys(DateTime date)
        {
            return CreateKeys(TimeHelper.TimestampFrom(date));
        }

        private int[] CreateKeys(int timestamp)
        {
            return CreateKeysFromTimestamp(timestamp.ToString());
        }

        private int[] CreateKeysFromTimestamp(string timestamp)
        {
            var items = new int[10];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = timestamp[i] - 48;
            }
            var last = items[^1];
            var offset = last % 2;
            for (var i = 0; i < items.Length; i += 2)
            {
                var pos = i + offset;
                if (pos >= items.Length)
                {
                    continue;
                }
                items[pos] = (items[pos] + last) % 10;
            }
            return items;
        }

        private int DictionaryLength() {
            return 51;
        }

        private string DictionaryCode(int code) {
            code -= 24;
            var rate = code % DictionaryLength();
            var prefix = code >= DictionaryLength() ? "0" : string.Empty;
            if (rate < 9)
            {
                return prefix + Convert.ToChar(rate + 49);
            }
            if (rate < 35)
            {
                return prefix + Convert.ToChar(rate + 88);
            }
            return prefix + Convert.ToChar(rate + 30);
        }

        private bool IsDictionaryCode(char code) {
            return code != '0';
        }

        private int DictionaryKey(string code) {
            var begin = 24;
            var i = 0;
            if (code.Length > 1)
            {
                begin += DictionaryLength();
                i = 1;
            }
            var ord = code[i];
            if (ord <= 57)
            {
                return begin + ord - 49;
            }
            if (ord <= 90)
            {
                return begin + ord - 30;
            }
            return begin + ord - 88;
        }
    }
}
