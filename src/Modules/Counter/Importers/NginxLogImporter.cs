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
    public class NginxLogImporter : ILogImporter
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
                filedItems = ["remote_addr", "remote_user", "time_local", "request", "status",
                "body_bytes_sent", "http_referer", "http_user_agent", "request_time",
                "upstream_response_time", "http_x_forwarded_for"];
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
                    case "time_local":
                        result.CreatedAt = TimeHelper.TimestampFrom(data[i]);
                        break;
                    case "remote_addr":
                        result.Ip = data[i];
                        break;
                    case "request":
                        ParseRequest(data[i], result);
                        break;
                    case "http_user_agent":
                        result.UserAgent = data[i];
                        break;
                    case "http_referer":
                        if (!string.IsNullOrWhiteSpace(data[i]) && Uri.TryCreate(data[i],
                            UriKind.Absolute, out var uri))
                        {
                            result.ReferrerHostname = uri.Host;
                            result.ReferrerPathname = uri.PathAndQuery;
                        }
                        break;
                    case "status":
                        result.StatusCode = int.Parse(data[i]);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }


        

        public string[] ParseField(string line)
        {
            // $remote_addr - $remote_user [$time_local] "$request" $status $body_bytes_sent "$http_referer" "$http_user_agent" "$http_x_forwarded_for"
            return Regex.Matches(line, @"\$([a-z_]+)").Select(i => i.Groups[1].Value).ToArray();
        }

        internal static string[] ParseValues(string line)
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

        internal static void ParseRequest(string line, LogEntity result)
        {
            var args = line.Split(' ');
            result.Method = args[0].ToUpper();
            if (Uri.TryCreate(args[1], UriKind.Absolute, out var uri))
            {
                result.Hostname = uri.Host;
                result.Pathname = uri.AbsolutePath;
                result.Queries = uri.Query;
            } else
            {
                args = args[1].Split('?', 2);
                result.Pathname = args[0];
                result.Queries = args.Length > 1 ? args[1] : string.Empty;
            }
        }

        
    }
}
