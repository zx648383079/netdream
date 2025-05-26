using NetDream.Shared.Helpers;
using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Exam
{
    public static class QuestionCompiler
    {
        public static JsonObject Generate(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return [];
            }
            var data = new JsonObject();
            foreach (var line in content.Split('\n'))
            {
                var items = line.Split('=', 2);
                if (items.Length < 2)
                {
                    continue;
                }
                data[items[0].Trim()] = CompilerValue(items[1].Trim(), data);
            }
            return data;
        }

        public static string StrReplace(string val, JsonObject data)
        {
            if (data.Count == 0 || string.IsNullOrWhiteSpace(val))
            {
                return val;
            }
            foreach (var item in data)
            {
                val = val.Replace(item.Key, item.Value.ToString());
            }
            return val;
        }

        public static JsonNode CompilerValue(string str, JsonObject data)
        {
            if (Validator.IsNumeric(str))
            {
                return str;
            }
            var match = Regex.Match(str, @"^(.+?)([\>\<\=\!]{1,3})(.+?)\?(.+?):(.+?)");
            if (match.Success)
            {
                var a = match.Groups[1].Value.Trim();
                var c = match.Groups[3].Value.Trim();
                if (data.TryGetPropertyValue(a, out var node))
                {
                    a = node.ToString();
                }
                if (data.TryGetPropertyValue(c, out node))
                {
                    c = node.ToString();
                }
                return CompilerCondition(a, match.Groups[2].Value, c)
                    ? CompilerValue(match.Groups[4].Value, data) 
                    : CompilerValue(match.Groups[5].Value, data);
            }
            match = Regex.Match(str, @"^(.+?)([\+\-*\/]{1,3})(.+?)");
            if (match.Success)
            {
                var a = match.Groups[1].Value.Trim();
                var c = match.Groups[3].Value.Trim();
                if (data.TryGetPropertyValue(a, out var node))
                {
                    a = node.ToString();
                }
                if (data.TryGetPropertyValue(c, out node))
                {
                    c = node.ToString();
                }
                return CompilerAssign(a, match.Groups[2].Value, c);
            }
            if (str.Contains("..."))
            {
                var args = str.Split("...");
                return Random.Shared.Next(int.Parse(args[0]), int.Parse(args[1]) + 1);
            }
            var items = str.Split(',');
            if (items.Length == 1)
            {
                return str.Trim();
            }
            return items[Random.Shared.Next(0, items.Length - 1)].Trim();
        }
        public static bool CompilerCondition(string arg, string con, string val)
        {
            return con switch
            {
                ">" => float.Parse(arg) > float.Parse(val),
                ">=" => float.Parse(arg) >= float.Parse(val),
                "<" => float.Parse(arg) < float.Parse(val),
                "<=" => float.Parse(arg) <= float.Parse(val),
                "<>" or "!=" => arg != val,
                "==" or "===" => arg == val,
                "+" => float.Parse(arg) + float.Parse(val) != 0,
                "-" => float.Parse(arg) - float.Parse(val) != 0,
                "*" => float.Parse(arg) == 0 || float.Parse(val) == 0,
                "/" => float.Parse(arg) == 0 || float.Parse(val) == 0,
                _ => false
            };
        }
        public static float CompilerAssign(string arg, string con, string val)
        {
            var a = float.Parse(arg);
            var c = float.Parse(val);
            return con switch
            {
                "+" => a + c,
                "-" => a - c,
                "*" => a == 0 || c == 0 ? 0 : (a * c),
                "/" => a == 0 || c == 0 ? 0 : (a / c),
                _ => 0
            };
        }

        public static string IntToChr(int val)
        {
            var str = string.Empty;
            if (val > 26)
            {
                str += IntToChr(val / 26);
            }
            return str + (char)(val % 26 + 64);
        }

    }
}
