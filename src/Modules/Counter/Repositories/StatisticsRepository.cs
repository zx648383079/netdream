using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Counter.Repositories
{
    public class StatisticsRepository(CounterContext db, IClientContext client)
    {
        public SubtotalResult Subtotal(string type)
        {
            var (startTime, endTime) = StateRepository.GetTimeRange(type);
            return new()
            {
                StageItems = GetStage(startTime, endTime, type)
            };
        }

        public DayResult Today()
        {
            var time = TimeHelper.TimestampFrom(DateTime.Today);
            var store = new StateRepository(db, client);
            var res = new DayResult();
            res.Today = store.StatisticsByTime(time, time + 86400);
            res.Yesterday = store.StatisticsByTime(time - 86400, time);
            var start = time + DateTime.Now.Hour * 3600;
            res.YesterdayHour = store.StatisticsByTime(
                start, start + 3600
            );
            var scale = (double)(client.Now - time) / 86400;
            res.ExpectToday = res.Today * scale;
            return res;
        }


        public StageItem[] GetStage(int startAt, int endAt, string type)
        {
            var data = new Dictionary<int, AnalysisGroup>();
            var items = db.Logs.Where(i => i.CreatedAt >= startAt && i.CreatedAt <= endAt)
                .Select(i => new LogEntity()
                {
                    Ip = i.Ip,
                    CreatedAt = i.CreatedAt,
                });
            foreach (var item in items)
            {
                var date = TimeHelper.TimestampTo(item.CreatedAt);
                var key = type switch
                {
                    "week" => (int)date.DayOfWeek,
                    "month" => date.Day,
                    _ => date.Hour,
                };
                if (!data.TryGetValue(key, out var group))
                {
                    group = new AnalysisGroup();
                    data.Add(key, group);
                }
                group.Count++;
                group.Ip.Add(item.Ip);
            }
            var start = 0;
            var end = 23;
            switch (type)
            {
                case "week":
                    end = 6;
                    break;
                case "month":
                    end = TimeHelper.MonthMaxDay(DateTime.Now);
                    break;
            }
            var res = new List<StageItem>();
            for (; start <= end; start++)
            {
                if (!data.TryGetValue(start, out var group))
                {
                    res.Add(new StageItem()
                    {
                        Date = start
                    });
                    continue;
                }
                res.Add(new StageItem()
                {
                    Date = start,
                    Pv = group.Count,
                    Uv = group.Ip.Count
                });
            }
            return [..res];
        }

        public CompareResult TrendAnalysis(string type = "today", int compare = 0)
        {
            var (startTime, endTime) = StateRepository.GetTimeRange(type);
            var res = new CompareResult();
            res.Items = GetStage(startTime, endTime, type);
            if (type == "week" || type == "month")
            {
                compare = 0;
            }
            if (compare == 1)
            {
                res.CompareItems = GetStage(startTime - 86400, endTime - 86400, type);
            }
            else if(compare == 2) {
                res.CompareItems = GetStage(startTime - 86400 * 7, endTime - 86400 * 7, type);
            }
            return res;
        }

    }
}
