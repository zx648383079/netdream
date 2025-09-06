using NetDream.Modules.Counter.Entities;
using NetDream.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace NetDream.Modules.Counter.Importers
{
    public class IISLogImporter : ILogImporter
    {

        public IEnumerable<LogEntity> Read(string[] filedItems, Stream input)
        {
            return ReadLine(filedItems, input);
        }

        private IEnumerable<LogEntity> ReadLine(string[] filedItems, Stream input)
        {
            var reader = new StreamReader(input);
            filedItems = [];
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith("#Fields"))
                {
                    filedItems = ParseField(line);
                    continue;
                }
                if (filedItems.Length == 0 || line.StartsWith('#'))
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
            var dateIndex = -1;
            var timeIndex = -1;
            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i])
                {
                    case "date":
                        dateIndex = i; 
                        break;
                    case "time":
                        timeIndex = i;
                        break;
                    case "c_ip":
                        result.Ip = data[i]; 
                        break;
                    case "cs_method":
                        result.Method = data[i].ToUpper();
                        break;
                    case "cs_uri_stem":
                        result.Pathname = data[i];
                        break;
                    case "cs_uri_query":
                        result.Queries = data[i];
                        break;
                    case "cs_user_agent":
                        result.UserAgent = data[i];
                        break;
                    case "cs_referer":
                        if (!string.IsNullOrWhiteSpace(data[i]) && Uri.TryCreate(data[i], 
                            UriKind.Absolute, out var uri))
                        {
                            result.ReferrerHostname = uri.Host;
                            result.ReferrerPathname = uri.PathAndQuery;
                        }
                        break;
                    case "sc_status":
                        result.StatusCode = int.Parse(data[i]);
                        break;
                    default:
                        break;
                }
            }
            if (dateIndex >= 0 && timeIndex >= 0)
            {
                result.CreatedAt = TimeHelper.TimestampFrom($"{data[dateIndex]} {data[timeIndex]}");
            } else if (dateIndex >= 0)
            {
                result.CreatedAt = TimeHelper.TimestampFrom($"{data[dateIndex]}");
            }
            else
            {
                return false;
            }
            return true;
        }

        private string[] ParseValues(string line)
        {
            return line.Split(' ').Select(i => i == "-" ? string.Empty : i).ToArray();
        }

        /// <summary>
        /// [0] date [1] time [2] s-ip [3] cs-method [4] cs-uri-stem 
        /// [5] cs-uri-query [6] s-port [7] cs-username [8] c-ip [9] cs(User-Agent) 
        /// [10] cs(Referer) [11] sc-status [12] sc-substatus [13] sc-win32-status [14] time-taken
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] ParseField(string line)
        {

            var i = line.IndexOf(':');
            if (i > 0)
            {
                line = line[(i + 1)..];
            }
            var res = new List<string>();
            foreach (var item in line.Split(' '))
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                res.Add(item.Replace('-', '_').Replace('(', '-').Replace(")", string.Empty).Trim().ToLower());
            }
            return [.. res];
        }
    }
}
