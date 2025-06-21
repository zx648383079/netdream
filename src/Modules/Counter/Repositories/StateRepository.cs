using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UAParser;

namespace NetDream.Modules.Counter.Repositories
{
    public class StateRepository(CounterContext db, IClientContext client)
    {
        public const byte STATUS_ENTER = 0;
        public const byte STATUS_LOADED = 1;
        public const byte STATUE_LEAVE = 2;

        public (int, int) GetTimeInput(string startAt = "today", string endAt = "")
        {
            if (startAt is "today" or "yesterday" or "week" or "month")
            {
                return GetTimeRange(startAt);
            }
            if (Validator.IsInt(startAt))
            {
                return (int.Parse(startAt), int.Parse(endAt));
            }
            return (TimeHelper.TimestampFrom(startAt), TimeHelper.TimestampFrom(endAt));
        }

        public static (int, int) GetTimeRange(string type)
        {
            var time = TimeHelper.TimestampFrom(DateTime.Today);
            if (type == "today")
            {
                return (time, time + 86400);
            }
            if (type == "yesterday")
            {
                return (time - 86400, time);
            }
            if (type == "week")
            {
                return (time - 6 * 85400, time + 86400);
            }
            if (type == "month")
            {
                return (time - 29 * 85400, time + 86400);
            }
            return (0, 0);
        }

        public TimeResult StatisticsByTime(int start_at, int end_at)
        {
            var pv = db.StayTimeLogs.Where(i => i.EnterAt >= start_at && i.EnterAt < end_at)
                .Count();
            var uv = db.StayTimeLogs.Where(i => i.EnterAt >= start_at && i.EnterAt < end_at)
                .GroupBy(i => i.SessionId).Count();
            var ip_count = db.StayTimeLogs.Where(i => i.EnterAt >= start_at && i.EnterAt < end_at)
                .GroupBy(i => i.Ip).Count();
            var jump_count = db.JumpLogs.Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at)
                .Count();
            var stay_time = db.StayTimeLogs.Where(i => i.EnterAt >= start_at 
                && i.EnterAt < end_at && i.LeaveAt > 0)
                .Average(i => i.LeaveAt - i.EnterAt);
            var next_time = db.Logs.Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at
                && i.Referrer.StartsWith(client.Host)).Count();
            return new TimeResult()
            {
                Pv = pv,
                Uv = uv,
                IpCount = ip_count,
                JumpCount = jump_count,
                StayTime = stay_time,
                NextTime = next_time,
            };
        }

        public IPage<StayTimeModel> CurrentStay(QueryForm form)
        {
            var items = db.StayTimeLogs.OrderByDescending(i => i.Id).ToPage(form);
            return new Page<StayTimeModel>(items) 
            {
                Items = items.Items.Select(FormatAgent<StayTimeModel>).ToArray()
            };
        }

        public IPage<ITrendAnalysis> AllUrl(int start_at, int end_at)
        {
            var items = MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Url))
                {
                    return null;
                }
                return i.Url;
            }, i => new UrlTrendAnalysis(i), 
            db.Logs.Select(i => new LogEntity()
            {
                Url = i.Url,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
            return new Page<ITrendAnalysis>(items);
        }

        public IPage<ITrendAnalysis> EnterUrl(int start_at, int end_at)
        {
            var items = MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Url))
                {
                    return null;
                }
                return i.Url;
            }, i => new UrlTrendAnalysis(i), 
            db.Logs.Where(i => i.Referrer == "" || !i.Referrer.Contains(client.Host)).Select(i => new LogEntity()
            {
                Url = i.Url,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
            return new Page<ITrendAnalysis>(items);
        }

        public ITrendAnalysis[] Domain(int start_at, int end_at)
        {
            return MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Url))
                {
                    return null;
                }
                var host = new Uri(i.Url).Host;
                if (string.IsNullOrEmpty(host))
                {
                    return null;
                }
                if (host.StartsWith("www."))
                {
                    return host[4..];
                }
                return host;
            }, i => new HostTrendAnalysis(i), 
            db.Logs.Select(i => new LogEntity()
            {
                Url = i.Url,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        private static string? IsSearch(string url)
        {
            var match = Regex.Match(url,  @"[\?&](q|wd|p|query|qkw|search|qr|string|keyword)\=([^&]*)");
            if (match.Success)
            {
                return match.Groups[2].Value;
            }
            return null;
        }

        public ITrendAnalysis[] SearchKeywords(int start_at, int end_at)
        {
            return MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Referrer))
                {
                    return null;
                }
                var words = IsSearch(i.Referrer);
                if (string.IsNullOrWhiteSpace(words))
                {
                    return null;
                }
                return words;
            }, i => new SearchTrendAnalysis(i), 
            db.Logs.Where(i => i.Referrer == "" || !i.Referrer.Contains(client.Host))
            .Select(i => new LogEntity()
            {
                Referrer = i.Referrer,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] SearchEngine(int start_at, int end_at)
        {
            return MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Referrer))
                {
                    return null;
                }
                var words = IsSearch(i.Referrer);
                if (string.IsNullOrWhiteSpace(words))
                {
                    return null;
                }
                var host = new Uri(i.Referrer).Host;
                if (string.IsNullOrEmpty(host))
                {
                    return null;
                }
                if (host.StartsWith("www."))
                {
                    return host[4..];
                }
                return host;
            }, i => new HostTrendAnalysis(i), 
            db.Logs.Where(i => i.Referrer == "" || !i.Referrer.Contains(client.Host))
            .Select(i => new LogEntity()
            {
                Referrer = i.Referrer,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] Host(int start_at, int end_at)
        {
            return MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Referrer))
                {
                    return null;
                }
                var host = new Uri(i.Referrer).Host;
                if (string.IsNullOrEmpty(host))
                {
                    return null;
                }
                if (host.StartsWith("www."))
                {
                    return host[4..];
                }
                return host;
            }, i => new HostTrendAnalysis(i), 
            db.Logs.Where(i => i.Referrer == "" || !i.Referrer.Contains(client.Host))
            .Select(i => new LogEntity()
            {
                Referrer = i.Referrer,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] AllSource(int start_at, int end_at)
        {
            return MapGroups(start_at, end_at, i => {
                if (string.IsNullOrWhiteSpace(i.Referrer))
                {
                    return "直接访问";
                }
                var host = new Uri(i.Referrer).Host;
                if (string.IsNullOrWhiteSpace(host))
                {
                    return "直接访问";
                }
                if (host.StartsWith("www."))
                {
                    return host[4..];
                }
                return host;
            }, i => new HostTrendAnalysis(i), db.Logs.Select(i => new LogEntity()
            {
                Referrer = i.Referrer,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public int GetJumpCount(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return 0;
            }
            return db.JumpLogs.Where(i => i.Referrer == url)
                .Count();
        }

        public string GetJumpScale(string url, int count)
        {
            if (string.IsNullOrWhiteSpace(url) || count < 1)
            {
                return "0%";
            }
            var total = db.JumpLogs.Where(i => !string.IsNullOrEmpty(i.Referrer))
                .Count();
            return total == 0 ? "100%" : (Math.Round((double)count * 100 / total, 2) + "%");
        }

        public int GetNextCount(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return 0;
            }
            return db.Logs.Where(i => i.Referrer == url).Count();
        }

        public double GetStayTime(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return 0;
            }
            return db.StayTimeLogs.Where(i => i.Url == url && i.LeaveAt > i.EnterAt)
                .Average(i => i.LeaveAt - i.EnterAt);
        }

        public ITrendAnalysis[] MapGroups(
            int start_at, int end_at, 
            Func<LogEntity, string?> checkFn,
            Func<string, ITrendAnalysis> createFn,
            IQueryable<LogEntity> query)
        {
            var items = query.Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at).ToArray();
            var data = new Dictionary<string, AnalysisGroup>();
            foreach (var item in items)
            {
                var host = checkFn.Invoke(item);
                if (host is null)
                {
                    continue;
                }
                if (!data.TryGetValue(host, out var group))
                {
                    group = new AnalysisGroup();
                    data.Add(host, group);
                }
                group.Count++;
                group.Ip.Add(item.Ip);
                group.Session.Add(item.SessionId);
            }
            var pv_total = data.Sum(i => i.Value.Count);
            return data.Select(i => {
                var item = createFn.Invoke(i.Key);
                item.IpCount = i.Value.Ip.Count;
                item.Pv = i.Value.Count;
                item.Uv = i.Value.Session.Count;
                item.Scale = Math.Round((double)item.Pv * 100 / pv_total, 2) + "%";
                return item;
            }).OrderByDescending(i => i.Pv).ToArray();
        }

        public IPage<JumpLogModel> Jump(int start_at, int end_at, QueryForm form)
        {
            var items = db.JumpLogs
                .Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
            return new Page<JumpLogModel>(items)
            {
                Items = items.Items.Select(FormatAgent<JumpLogModel>).ToArray()
            };
        }

        public T FormatAgent<T>(IUserAgent item)
            where T : class, IUserAgentFormatted, new()
        {
            if (string.IsNullOrWhiteSpace(item.UserAgent))
            {
                return item.CopyTo<T>();
            }
            var data = item.CopyTo<T>();
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(item.UserAgent);
            data.Device = [clientInfo.Device.Family, clientInfo.Device.Brand];
            data.Os = [clientInfo.OS.Family, $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}"];
            data.Browser = [clientInfo.UA.Family, $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}"];
            return data;
        }
    }
}
