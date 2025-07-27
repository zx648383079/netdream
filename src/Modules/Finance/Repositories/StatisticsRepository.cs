using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class StatisticsRepository(FinanceContext db, IClientContext client)
    {
        public IncomeStatisticsResult GetIncome(string month)
        {
            if (!DateTime.TryParse(month, out var now))
            {
                now = DateTime.Now;
            }
            var (startAt, endAt) = TimeHelper.MonthRange(now);

            var res = new IncomeStatisticsResult()
            {
                Month = startAt.ToString("yyyy-MM"),
                DayLength = TimeHelper.MonthMaxDay(startAt),
                LogList = db.Log.Where(i => i.UserId == client.UserId && i.HappenedAt >= startAt && i.HappenedAt < endAt)
                .OrderByDescending(i => i.HappenedAt).ToArray()
            };
            return res;
        }

        public MoneyStatisticsResult GetMoney()
        {
            var res = new MoneyStatisticsResult()
            {
                AccountList = db.Account.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray(),
                ProductList = db.Product.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray(),
                ProjectList = db.Project.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray()
            };
            return res;
        }

        public StatisticsResult Subtotal(StatisticsForm form)
        {
            var typeStart = DateTime.Now;
            if (!DateTime.TryParse(form.StartAt, out var startAt))
            {
                startAt = DateTime.MinValue;
            } else
            {
                typeStart = startAt;
            }
            if (!DateTime.TryParse(form.EndAt, out var endAt))
            {
                endAt = DateTime.MaxValue;
            }
            var (currentStart, currentEnd, lastStart, lastEnd) = GetTimeRange(typeStart, form.Type);
            var res = new StatisticsResult()
            {
                MoneyTotal = db.Account.Where(i => i.UserId == client.UserId)
                .Sum(i => i.Money + i.FrozenMoney),
                IncomeCount = db.Log.Where(i => 
                    i.UserId == client.UserId 
                    && i.HappenedAt >= currentStart && i.HappenedAt < currentEnd
                    && i.Type == LogRepository.TYPE_INCOME)
                    .Count(),
                IncomeCurrent = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.HappenedAt >= currentStart && i.HappenedAt < currentEnd
                    && i.Type == LogRepository.TYPE_INCOME)
                    .Sum(i => i.Money),
                IncomeLast = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.HappenedAt >= lastStart && i.HappenedAt < lastEnd
                    && i.Type == LogRepository.TYPE_INCOME)
                    .Sum(i => i.Money),
                IncomeTotal = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.Type == LogRepository.TYPE_INCOME)
                    .When(startAt != DateTime.MinValue, i => i.HappenedAt >= startAt)
                    .When(endAt != DateTime.MaxValue, i => i.HappenedAt < endAt)
                    .Sum(i => i.Money),
                ExpenditureCount = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.HappenedAt >= currentStart && i.HappenedAt < currentEnd
                    && i.Type == LogRepository.TYPE_EXPENDITURE)
                    .Count(),
                ExpenditureCurrent = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.HappenedAt >= currentStart && i.HappenedAt < currentEnd
                    && i.Type == LogRepository.TYPE_EXPENDITURE)
                    .Sum(i => i.Money),
                ExpenditureLast = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.HappenedAt >= lastStart && i.HappenedAt < lastEnd
                    && i.Type == LogRepository.TYPE_EXPENDITURE)
                    .Sum(i => i.Money),
                ExpenditureTotal = db.Log.Where(i =>
                    i.UserId == client.UserId
                    && i.Type == LogRepository.TYPE_EXPENDITURE)
                    .When(startAt != DateTime.MinValue, i => i.HappenedAt >= startAt)
                    .When(endAt != DateTime.MaxValue, i => i.HappenedAt < endAt)
                    .Sum(i => i.Money)
            };
            var data = db.Log.Where(i =>
                    i.UserId == client.UserId && (i.Type == LogRepository.TYPE_EXPENDITURE || i.Type == LogRepository.TYPE_INCOME))
                    .When(startAt != DateTime.MinValue, i => i.HappenedAt >= startAt)
                    .When(endAt != DateTime.MaxValue, i => i.HappenedAt < endAt)
                    .Select(i => new LogEntity()
                    {
                        Type = i.Type,
                        Money = i.Money,
                        HappenedAt = i.HappenedAt,
                    }).ToArray();
            var dateFormat = form.Type switch
            {
                0 => "yyyy-MM-dd",
                1 or 2 or 3 => "yyyy-MM",
                _ => "yyyy"
            };
            var maps = new Dictionary<string, LogStageItem>();
            foreach (var item in data)
            {
                var date = item.HappenedAt.ToString(dateFormat);
                if (!maps.TryGetValue(date, out var log))
                {
                    log = new LogStageItem()
                    {
                        Date = date
                    };
                    maps.Add(date, log);
                }
                if (item.Type ==  LogRepository.TYPE_EXPENDITURE)
                {
                    log.Expenditure += item.Money;
                } else
                {
                    log.Income += item.Money;
                }
            }
            res.StageItems = maps.Values.ToArray();
            return res;
        }

        private (DateTime currentStart, DateTime currentEnd, DateTime lastStart, DateTime lastEnd) GetTimeRange(DateTime now, int type)
        {
            switch (type)
            {
                case 0: // 天
                    {
                        var current = now.Date;
                        return (current, current.AddDays(1), current.AddDays(-1), current);
                    }
                case 1: // 月
                    {
                        var current = new DateTime(now.Year, now.Month, 1);
                        return (current, current.AddMonths(1), current.AddMonths(-1), current);
                    }
                case 2: // 一季度
                    {
                        var current = new DateTime(now.Year, (int)Math.Floor((double)(now.Month - 1) / 3) * 3 + 1, 1);
                        return (current, current.AddMonths(3), current.AddMonths(-3), current);
                    }
                case 3: // 半年
                    {
                        var current = new DateTime(now.Year, now.Month > 6 ? 7 : 1, 1);
                        return (current, current.AddMonths(6), current.AddMonths(-6), current);
                    }
                default: // 年
                    {
                        var current = new DateTime(now.Year, 1, 1);
                        return (current, current.AddYears(1), current.AddYears(-1), current);
                    }
            }
        }
    }
}
