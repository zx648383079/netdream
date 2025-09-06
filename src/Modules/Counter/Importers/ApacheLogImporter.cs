using NetDream.Modules.Counter.Entities;
using NetDream.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Counter.Importers
{
    public class ApacheLogImporter : ILogImporter
    {
        public IEnumerable<LogEntity> Read(string[] filedItems, Stream input)
        {
            return ReadLine(filedItems, input);
        }

        private IEnumerable<LogEntity> ReadLine(string[] filedItems, Stream input)
        {
            var reader = new StreamReader(input);
            if (filedItems.Length == 0)
            {
                filedItems = ["h", "l", "u", "t", "r", ">s", "b"];
            }
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (TryParse(line, filedItems, out var item))
                {
                    yield return item;
                }
            }
        }

        private bool TryParse(string line, string[] fields, [NotNullWhen(true)] out LogEntity? result)
        {
            var data = ParseValues(line);
            if (data.Length != fields.Length)
            {
                result = null;
                return false;
            }
            result = new();
            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i])
                {
                    case "t":
                        result.CreatedAt = TimeHelper.TimestampFrom(data[i]);
                        break;
                    case "a":
                    case "h":
                        result.Ip = data[i];
                        break;
                    case "r":
                        NginxLogImporter.ParseRequest(data[i], result);
                        break;
                    case "q":
                        result.Queries = data[i];
                        break;
                    case "m":
                        result.Method = data[i].ToUpper();
                        break;
                    case "U":
                        result.Pathname = data[i];
                        break;
                    case "{User-Agent}i":
                        result.UserAgent = data[i];
                        break;
                    case "{Referer}i":
                        if (!string.IsNullOrWhiteSpace(data[i]) && Uri.TryCreate(data[i],
                            UriKind.Absolute, out var uri))
                        {
                            result.ReferrerHostname = uri.Host;
                            result.ReferrerPathname = uri.PathAndQuery;
                        }
                        break;
                    case ">s":
                        result.StatusCode = int.Parse(data[i]);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }


        private string[] ParseValues(string line)
        {
            var res = new List<string>();
            var i = 0;
            while (i < line.Length)
            {
                var next = line[i] switch
                {
                    '"' => line.IndexOf('"', i++),
                    '[' => line.IndexOf(']', i++),
                    '<' => line.IndexOf('>', i++),
                    _ => line.IndexOf(' ', i)
                };
                if (next < i)
                {
                    res.Add(line[i..]);
                    break;
                }
                res.Add(line[i..next]);
                i = next + 1;
                while (i < line.Length && line[i] == ' ')
                {
                    i++;
                }
            }
            return res.ToArray();
        }

        public string[] ParseField(string line)
        {
            // %h %l %u %t "%r" %>s %b %{Referer}i\ \"%{User-Agent}i\
            return Regex.Matches(line, @"%([\{\}a-z_\->]+)").Select(i => i.Groups[1].Value).ToArray();
        }
    }
}
