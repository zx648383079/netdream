using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Forms;
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
            var uv = 0;//db.StayTimeLogs.Where(i => i.EnterAt >= start_at && i.EnterAt < end_at)
                //.GroupBy(i => i.SessionId).Count();
            var ip_count = 0;// db.StayTimeLogs.Where(i => i.EnterAt >= start_at && i.EnterAt < end_at)
                //.GroupBy(i => i.Ip).Count();
            var jump_count = db.JumpLogs.Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at)
                .Count();
            var stay_time = db.StayTimeLogs.Where(i => i.EnterAt >= start_at 
                && i.EnterAt < end_at && i.LeaveAt > 0)
                .Average(i => i.LeaveAt - i.EnterAt);
            var next_time = db.Logs.Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at
                && i.ReferrerHostname == client.Host).Count();
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
            var items = db.StayTimeLogs.OrderByDescending(i => i.Id).ToPage(form, i => i.SelectAs());
            IncludeClient(items.Items);
            return items;
        }

        public IPage<ITrendAnalysis> AllUrl(AnalysisQueryForm form)
        {
            var items = MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.Pathname))
                {
                    return null;
                }
                return i.Pathname;
            }, i => new UrlTrendAnalysis(i), 
            db.Logs.Select(i => new LogEntity()
            {
                Pathname = i.Pathname,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
            return new Page<ITrendAnalysis>(items);
        }

        public IPage<ITrendAnalysis> EnterUrl(AnalysisQueryForm form)
        {
            var items = MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.Pathname))
                {
                    return null;
                }
                return i.Pathname;
            }, i => new UrlTrendAnalysis(i), 
            db.Logs.Where(i => i.ReferrerHostname == "" || i.ReferrerHostname != client.Host).Select(i => new LogEntity()
            {
                Pathname = i.Pathname,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
            return new Page<ITrendAnalysis>(items);
        }

        public ITrendAnalysis[] Domain(AnalysisQueryForm form)
        {
            return MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.Hostname))
                {
                    return null;
                }
                var host = i.Hostname;
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
                Hostname = i.Hostname,
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

        public ITrendAnalysis[] SearchKeywords(AnalysisQueryForm form)
        {
            return MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.ReferrerPathname))
                {
                    return null;
                }
                var words = IsSearch(i.ReferrerPathname);
                if (string.IsNullOrWhiteSpace(words))
                {
                    return null;
                }
                return words;
            }, i => new SearchTrendAnalysis(i), 
            db.Logs.Where(i => i.ReferrerHostname == "" || i.ReferrerHostname != client.Host)
            .Select(i => new LogEntity()
            {
                ReferrerPathname = i.ReferrerPathname,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] SearchEngine(AnalysisQueryForm form)
        {
            return MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.ReferrerPathname))
                {
                    return null;
                }
                var words = IsSearch(i.ReferrerPathname);
                if (string.IsNullOrWhiteSpace(words))
                {
                    return null;
                }
                var host = i.ReferrerHostname;
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
            db.Logs.Where(i => i.ReferrerHostname == "" || i.ReferrerHostname != client.Host)
            .Select(i => new LogEntity()
            {
                ReferrerHostname = i.ReferrerHostname,
                ReferrerPathname = i.ReferrerPathname,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] Host(AnalysisQueryForm form)
        {
            return MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.ReferrerHostname))
                {
                    return null;
                }
                var host = i.ReferrerHostname;
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
            db.Logs.Where(i => i.ReferrerHostname == "" || i.ReferrerHostname != client.Host)
            .Select(i => new LogEntity()
            {
                ReferrerHostname = i.ReferrerHostname,
                Ip = i.Ip,
                SessionId = i.SessionId,
            }));
        }

        public ITrendAnalysis[] AllSource(AnalysisQueryForm form)
        {
            return MapGroups(form, i => {
                if (string.IsNullOrWhiteSpace(i.ReferrerHostname))
                {
                    return "直接访问";
                }
                var host = i.ReferrerHostname;
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
                ReferrerHostname = i.ReferrerHostname,
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
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return 0;
            }
            return db.Logs.Where(i => i.ReferrerHostname == uri.Host && i.ReferrerPathname == uri.PathAndQuery).Count();
        }

        public double GetStayTime(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return 0;
            }
            return 0;
            //return db.StayTimeLogs.Where(i => i.Url == url && i.LeaveAt > i.EnterAt)
            //    .Average(i => i.LeaveAt - i.EnterAt);
        }

        public ITrendAnalysis[] MapGroups(
            AnalysisQueryForm form, 
            Func<LogEntity, string?> checkFn,
            Func<string, ITrendAnalysis> createFn,
            IQueryable<LogEntity> query)
        {
            var start_at = TimeHelper.TimestampFrom(form.StartAt);
            var end_at = TimeHelper.TimestampFrom(form.EndAt);
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

        public IPage<JumpLogModel> Jump(AnalysisQueryForm form)
        {
            var start_at = TimeHelper.TimestampFrom(form.StartAt);
            var end_at = TimeHelper.TimestampFrom(form.EndAt);
            var items = db.JumpLogs
                .Where(i => i.CreatedAt >= start_at && i.CreatedAt < end_at)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            return items;
        }

        public void IncludeClient(IWithClientModel[] items)
        {
            var idItems = items.Select(item => item.LogId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Logs.Where(i => idItems.Contains(i.Id))
                .Select(i => new ClientLabelItem()
                {
                    Ip = i.Ip,
                    UserAgent = i.UserAgent,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            Format(data.Values);
            foreach (var item in items)
            {
                if (item.LogId > 0 && data.TryGetValue(item.LogId, out var res))
                {
                    item.Client = res;
                }
            }
        }

        public void Format(IEnumerable<IUserAgentFormatted> items)
        {
            var uaParser = Parser.GetDefault();
            foreach (var item in items)
            {
                var clientInfo = uaParser.Parse(item.UserAgent);
                item.Device = [clientInfo.Device.Family, clientInfo.Device.Brand];
                item.Os = [clientInfo.OS.Family, $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}"];
                item.Browser = [clientInfo.UA.Family, $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}"];
            }
        }
    }
}
