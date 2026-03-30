using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Forms;
using NetDream.Modules.Counter.Importers;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Counter.Repositories
{
    public class AnalysisRepository(
        CounterContext db, 
        IClientContext client)
    {

        public IPage<LogEntity> LogList(LogQueryForm form)
        {
            var startAt = string.IsNullOrWhiteSpace(form.StartAt) ? 0 : TimeHelper.TimestampFrom(form.StartAt);
            var endAt = string.IsNullOrWhiteSpace(form.EndAt) ? 0 : TimeHelper.TimestampFrom(form.EndAt);
            var jumpTo = string.IsNullOrWhiteSpace(form.Goto) ? 0 : TimeHelper.TimestampFrom(form.Goto);
            if (jumpTo > 0)
            {
                var count = db.Logs.Search(form.Keywords, "queries", "pathname")
                    .When(form.UserAgent, i => i.UserAgent == form.UserAgent)
                    .When(form.Hostname, i => i.Hostname == form.Hostname)
                    .When(form.Pathname, i => i.Pathname == form.Pathname)
                    .When(endAt > startAt, i => i.CreatedAt < endAt)
                    .Where(i => i.CreatedAt >= jumpTo)
                    .Count();
                form.Page = (int)Math.Ceiling((float)count / form.PerPage);
            }
            return db.Logs.Search(form.Keywords, "queries", "pathname")
                .When(form.UserAgent, i => i.UserAgent == form.UserAgent)
                .When(form.Hostname, i => i.Hostname == form.Hostname)
                .When(form.Pathname, i => i.Pathname == form.Pathname)
                .When(startAt > 0, i => i.CreatedAt >= startAt)
                .When(endAt > startAt, i => i.CreatedAt < endAt)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
        }

        public IOperationResult<int> LogImport(LogImportForm data, IUploadFile file)
        {
            ILogImporter importer = data.Engine?.ToLower() switch
            {
                "iis" => new IISLogImporter(),
                "apache" => new ApacheLogImporter(),
                _ => new NginxLogImporter()
            };
            if (string.IsNullOrWhiteSpace(data.Hostname))
            {
                data.Hostname = file.Name;
            }
            var fieldNames = importer.ParseField(data.FieldNames);
            var lastAt = db.Logs.Where(i => i.Hostname == data.Hostname)
                .Max(i => i.CreatedAt);
            using var fs = file.OpenRead();
            var count = 0;
            foreach (var item in importer.Read(fieldNames, fs))
            {
                if (item.CreatedAt <= lastAt)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(item.Hostname))
                {
                    item.Hostname = data.Hostname;
                }
                db.Logs.Add(item);
                count++;
            }
            db.SaveChanges();
            return OperationResult.Ok(count);
        }

        public IOperationResult<AnalysisFlagEntity> Mark(AnalysisMarkForm data)
        {
            var model = db.AnalysisFlags.Where(i => i.UserId == client.UserId && i.ItemType == data.Type && i.ItemValue == data.Value)
                .FirstOrDefault();
            if (model is not null)
            {
                return OperationResult.Ok(model);
            }
            model = new AnalysisFlagEntity()
            {
                ItemType = (byte)data.Type,
                ItemValue = data.Value,
                UserId = client.UserId,
                CreatedAt = client.Now
            };
            db.AnalysisFlags.Add(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Add(byte itemType, int itemId, byte action)
        {
            db.Logs.Add(new LogEntity()
            {
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                UserId = environment.UserId,
                CreatedAt = environment.Now
            });
            db.SaveChanges();
        }

        public bool AddUnique(byte itemType, int itemId, byte action)
        {
            if (Has(itemType, itemId, action))
            {
                return false;
            }
            Add(itemType, itemId, action);
            return true;
        }

        protected bool Has(byte itemType, int itemId, byte action)
        {
            return db.Logs.Where(i => i.ItemType == itemType && i.ItemId == itemId && i.Action == action && i.UserId == environment.UserId).Any();
        }

        protected bool HasCount(byte itemType, int itemId, byte action, string to)
        {
            return db.DayLogs.Where(i => i.ItemType == itemType && i.ItemId == itemId && i.Action == action && i.HappenDay == to).Any();
        }

        protected void AddCount(byte itemType, int itemId, byte action,
            string to, int start, int end)
        {
            if (HasCount(itemType, itemId, action, to))
            {
                return;
            }
            var count = db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType
            && i.Action == action && i.CreatedAt >= start && i.CreatedAt <= end).Count();
            db.DayLogs.Add(new DayLogEntity()
            {
                HappenDay = to,
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                HappenCount = count,
                CreatedAt = environment.Now,
            });
            db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType
            && i.Action == action && i.CreatedAt >= start && i.CreatedAt <= end).ExecuteDelete();
            db.SaveChanges();
        }

        protected void MergeCount(byte itemType, int itemId,
            byte action, string to, string[] from)
        {
            if (HasCount(itemType, itemId, action, to))
            {
                return;
            }
            var count = db.DayLogs.Where(i => i.ItemId == itemId && i.ItemType == itemType
                    && i.Action == action && from.Contains(i.HappenDay))
                .Sum(i => i.HappenCount);
            db.DayLogs.Add(new DayLogEntity()
            {
                HappenDay = to,
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                HappenCount = count,
                CreatedAt = environment.Now,
            });
            db.DayLogs.Where(i => i.ItemId == itemId && i.ItemType == itemType
                    && i.Action == action && from.Contains(i.HappenDay)).ExecuteDelete();
        }

        public int Count(byte itemType, int itemId, byte action)
        {
            return db.Logs.Where(i => i.ItemType == itemType && i.ItemId == itemId && i.Action == action).Count()
                + db.DayLogs.Where(i => i.ItemType == itemType && i.ItemId == itemId && i.Action == action)
                .Sum(i => i.HappenCount);
        }

        public int DayCount(byte itemType, int itemId, byte action, string day)
        {
            var query = db.DayLogs.Where(i => i.ItemType == itemType && i.Action == action && i.HappenDay == day);
            if (itemId > 0)
            {
                query = query.Where(i => i.ItemId == itemId);
            }
            var log = query.Single();
            if (log is not null)
            {
                return log.HappenCount;
            }
            var start = TimeHelper.TimestampFrom(DateTime.Parse(day + " 00:00:00"));
            var end = start + 86400;
            var q = db.Logs.Where(i => i.ItemType == itemType && i.Action == action && i.CreatedAt >= start && i.CreatedAt < end); ;
            if (itemId > 0)
            {
                q = q.Where(i => i.ItemId == itemId);
            }
            return q.Count();
        }

        public int TodayCount(byte itemType, int itemId, byte action)
        {
            return DayCount(itemType, itemId, action, DateTime.Now.ToString("yyyy-MM-dd"));
        }

        public int YesterdayCount(byte itemType, int itemId, byte action)
        {
            return DayCount(itemType, itemId, action,
                DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
        }

        public IList<OptionItem<int>> CountByHour(byte itemType, int itemId, byte action)
        {
            var start = TimeHelper.TimestampFrom(DateTime.Today);
            var data = db.Logs.Where(i => i.ItemType == itemType && i.ItemId == itemId && i.Action == action
            && i.CreatedAt >= start).ToArray();
            var max = DateTime.Now.Hour;
            var items = new OptionItem<int>[max + 1];
            for (var i = 0; i <= max; i++)
            {
                items[i] = new OptionItem<int>(i.ToString(), 0);
            }
            foreach (var item in data)
            {
                var h = TimeHelper.TimestampTo(item.CreatedAt).Hour.ToString();
                foreach (var it in items)
                {
                    if (h == it.Name)
                    {
                        it.Value++;
                    }
                }
            }
            return items;
        }

        public IList<OptionItem<int>> CountByDay(byte itemType, int itemId, byte action)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var max = DateTime.Now.Day;
            var items = new OptionItem<int>[max];
            for (var i = 0; i < max; i++)
            {
                items[i] = new OptionItem<int>(string.Format("{0}-{1:d2}-{2:d2}", year, month, i + 1), 0);
            }
            var dayItems = items.Select(i => i.Name);
            var data = db.DayLogs.Where(i => i.ItemType == itemType
                    && i.Action == action && dayItems.Contains(i.HappenDay))
                .ToArray();
            foreach (var item in data)
            {
                foreach (var it in items)
                {
                    if (item.HappenDay == it.Name)
                    {
                        it.Value += item.HappenCount;
                    }
                }
            }
            foreach (var it in items)
            {
                // 移除年
                it.Name = it.Name[5..];
            }
            return items;
        }

        public IList<OptionItem<int>> CountByMonth(byte itemType, int itemId, byte action)
        {
            var max = DateTime.Now.Month;
            var items = new OptionItem<int>[max];
            var year = DateTime.Now.Year;
            for (var i = 0; i < max; i++)
            {
                items[i] = new OptionItem<int>(string.Format("{0}-{1:d2}", year, i + 1), 0);
            }
            var dayItems = items.Select(i => i.Name);
            var data = db.DayLogs.Where(i => i.ItemType == itemType
                    && i.Action == action && dayItems.Contains(i.HappenDay))
                .ToArray();
            foreach (var item in data)
            {
                foreach (var it in items)
                {
                    if (item.HappenDay == it.Name)
                    {
                        it.Value += item.HappenCount;
                    }
                }
            }
            return items;
        }

        public IList<OptionItem<int>> CountByYear(byte itemType, int itemId, byte action)
        {
            return [];
        }

        public LogCount[] SortByDay(byte itemType, byte action)
        {
            var now = TimeHelper.TimestampFrom(DateTime.Today);
            return db.Logs.Where(i => i.ItemType == itemType
            && i.Action == action && i.CreatedAt >= now)
                .GroupBy(i => i.ItemId)
                .Select(i => new LogCount()
                {
                    ItemId = i.Key,
                    Count = i.Count()
                }).OrderByDescending(i => i.Count).ToArray();
        }

        public LogCount[] SortByWeek(byte itemType, byte action)
        {
            var (start, end) = TimeHelper.WeekRange(DateTime.Now);
            var dayItems = TimeHelper.RangeDate(start, end);
            return db.DayLogs.Where(i => i.ItemType == itemType && i.Action == action && dayItems.Contains(i.HappenDay))
                .GroupBy(i => i.ItemId)
                .Select(i => new LogCount()
                {
                    ItemId = i.Key,
                    Count = i.Sum(j => j.HappenCount)
                }).OrderByDescending(i => i.Count).ToArray();
        }

        public LogCount[] SortByMonth(byte itemType, byte action)
        {
            var (start, end) = TimeHelper.MonthRange(DateTime.Now);
            var dayItems = TimeHelper.RangeDate(start, end);
            return db.DayLogs.Where(i => i.ItemType == itemType && i.Action == action && dayItems.Contains(i.HappenDay))
                .GroupBy(i => i.ItemId)
                .Select(i => new LogCount()
                {
                    ItemId = i.Key,
                    Count = i.Sum(j => j.HappenCount)
                }).OrderByDescending(i => i.Count).ToArray();
        }

        public IList<LogCount> SortByYear(byte itemType, byte action)
        {
            return [];
        }

        public void MergeTask(byte itemType, int itemId, byte action)
        {
            var today = TimeHelper.TimestampFrom(DateTime.Today);
            var yesterday = today - 86400;
            AddCount(itemType, itemId, action,
                TimeHelper.TimestampTo(yesterday).ToString("yyyy-MM-dd"),
                yesterday, today - 1);
            // 合并上个月
            var lastMonth = DateTime.Now.AddMonths(-1);
            var month = lastMonth.ToString("yyyy-MM");
            var dayItems = new string[TimeHelper.MonthMaxDay(lastMonth)];
            for (var i = 0; i < dayItems.Length; i++)
            {
                dayItems[i] = string.Format("{0}-{1:d2}", month, i + 1);
            }
            MergeCount(itemType, itemId, action, month, [.. dayItems]);
            // 合并去年
            var year = DateTime.Now.Year - 1;
            var monthItems = new string[12];
            for (var i = 0; i < monthItems.Length; i++)
            {
                monthItems[i] = string.Format("{0}-{1:d2}", year, i + 1);
            }
            MergeCount(itemType, itemId, action, year.ToString(), monthItems);
        }
    }
}
