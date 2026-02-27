using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class BudgetRepository(FinanceContext db, IClientContext client)
    {
        public const byte CYCLE_ONCE = 0;
        public const byte CYCLE_DAY = 1;
        public const byte CYCLE_WEEK = 2;
        public const byte CYCLE_MONTH = 3;
        public const byte CYCLE_YEAR = 4;
        public IPage<BudgetListItem> GetList(QueryForm form)
        {
            var res = db.Budget.Where(i => i.UserId == client.UserId && i.DeletedAt == 0)
                .OrderByDescending(i => i.Id)
                .Select(i => new BudgetListItem()
                {
                    Id = i.Id,
                    Budget = i.Budget,
                    Spent = i.Spent,
                    Cycle = i.Cycle,
                    Name = i.Name,
                    CreatedAt = i.CreatedAt,
                })
                .ToPage(form);
            foreach (var item in res.Items)
            {
                ComputeSpent(item);
            }
            return res;
        }

        public ListLabelItem[] All()
        {
            return db.Budget.Where(i => i.UserId == client.UserId && i.DeletedAt == 0)
                .OrderByDescending(i => i.Id).Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public void RefreshSpent()
        {
            var items = db.Budget.Where(i => i.UserId == client.UserId).ToArray();
            foreach (var item in items)
            {
                RefreshSpent(item);
            }
            db.SaveChanges();
        }

        public void RefreshSpent(int id)
        {
            var model = db.Budget.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return;
            }
            RefreshSpent(model);
            db.Budget.Save(model, client.Now);
            db.SaveChanges();
        }

        public void RefreshSpent(BudgetEntity budget)
        {
            var time = budget.UpdatedAt;
            budget.UpdatedAt = 0;
            if (budget.Cycle == CYCLE_ONCE)
            {
                budget.Spent = db.Log.Where(i => i.UserId == client.UserId 
                && i.BudgetId == budget.Id && i.Type == LogRepository.TYPE_EXPENDITURE)
                    .Sum(i => i.Money);
            }
            ComputeSpent(budget);
            if (budget.UpdatedAt < 1)
            {
                budget.UpdatedAt = time;
            }
        }

        public void ComputeSpent(IBudget budget)
        {
            if (budget.Cycle == CYCLE_ONCE)
            {
                return;
            }
            var (startAt, endAt) = GetTimeRange(budget.Cycle);
            if (budget.UpdatedAt >= TimeHelper.TimestampFrom(startAt) && budget.UpdatedAt <= TimeHelper.TimestampFrom(endAt))
            {
                return;
            }
            budget.Spent = db.Log.Where(i => i.HappenedAt >= startAt && i.HappenedAt < endAt && i.UserId == client.UserId 
            && i.BudgetId == budget.Id && i.Type == LogRepository.TYPE_EXPENDITURE)
                .Sum(i => i.Money);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<BudgetEntity> Get(int id)
        {
            var model = db.Budget.Where(i => i.UserId == client.UserId && i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<BudgetEntity>.Fail("预算不存在");
            }
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<BudgetEntity> Save(BudgetForm data)
        {
            BudgetEntity? model;
            if (data.Id > 0)
            {
                model = db.Budget.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<BudgetEntity>("预算不存在");
            }
            model.Name = data.Name;
            model.Cycle = data.Cycle;
            model.Budget = data.Budget;
            model.UserId = client.UserId;
            db.Budget.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 软删除产品
        /// </summary>
        /// <param name="id"></param>
        public void SoftDelete(int id)
        {
            db.Budget.Where(i => i.UserId == client.UserId && i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now));
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            db.Budget.Where(i => i.UserId == client.UserId && i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }



        /// <summary>
        /// 获取统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<BudgetStatistics> Statistics(int id)
        {
            var model = db.Budget.Where(i => i.UserId == client.UserId && i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<BudgetStatistics>("预算不存在");
            }
            var res = new BudgetStatistics(model);
            if (model.Cycle == CYCLE_ONCE)
            {
                res.LogList.Add(0, model.Spent);
            } else if (model.Cycle == CYCLE_DAY)
            {
                GetLogByDay(res.LogList, id);
            }
            else if (model.Cycle == CYCLE_WEEK)
            {
                GetLogByWeek(res.LogList, id);
            }
            else if (model.Cycle == CYCLE_MONTH)
            {
                GetLogByMonth(res.LogList, id);
            }
            else if (model.Cycle == CYCLE_YEAR)
            {
                GetLogByYear(res.LogList, id);
            }
            return OperationResult.Ok(res);
        }

        private void GetLogByYear(SortedDictionary<int, decimal> res, int id)
        {
            var now = DateTime.Now;
            var data = db.Log.Where(i => i.UserId == client.UserId
                && i.BudgetId == id)
                .GroupBy(i => QueryExtensions.DateFormat(i.HappenedAt, "%Y"))
                .Select(i => new {
                    Date = i.Key,
                    Money = i.Sum(j => j.Money)
                }).ToArray();
            foreach (var item in data)
            {
                res.Add(int.Parse(item.Date), item.Money);
            }
            var max = now.Month;
            for (int i = res.Keys.Min(); i <= now.Year; i++)
            {
                res.TryAdd(i, 0);
            }
        }

        private void GetLogByMonth(SortedDictionary<int, decimal> res, int id)
        {
            var now = DateTime.Now;
            var endAt = new DateTime(now.Year + 1, 1, 1);
            var startAt = new DateTime(now.Year - (now.Month < 6 ? 2 : 1), 1, 1);
            var data = db.Log.Where(i => i.UserId == client.UserId
                && i.BudgetId == id && i.HappenedAt >= startAt && i.HappenedAt < endAt)
                .GroupBy(i => QueryExtensions.DateFormat(i.HappenedAt, "%Y%m"))
                .Select(i => new {
                    Date = i.Key,
                    Money = i.Sum(j => j.Money)
                }).ToArray();
            foreach (var item in data)
            {
                res.Add(int.Parse(item.Date), item.Money);
            }
            var max = now.Month;
            for (int i = 1; i <= 12; i++)
            {
                if (now.Month < 6)
                {
                    res.TryAdd(startAt.Year * 100 + i, 0);
                }
                res.TryAdd((now.Year - 1) * 100 + i, 0);
                if (i <= max)
                {
                    res.TryAdd(now.Year * 100 + i, 0);
                }
            }
        }

        private void GetLogByWeek(IDictionary<int, decimal> res, int id)
        {
            var now = DateTime.Now;
            var endAt = new DateTime(now.Year + 1, 1, 1);
            var startAt = new DateTime(now.Year - (now.Month < 6 ? 1 : 0), 1, 1);
            var data = db.Log.Where(i => i.UserId == client.UserId
                && i.BudgetId == id && i.HappenedAt >= startAt && i.HappenedAt < endAt)
                .GroupBy(i => QueryExtensions.DateFormat(i.HappenedAt, "%Y%u"))
                .Select(i => new {
                    Date = i.Key,
                    Money = i.Sum(j => j.Money)
                }).ToArray();
            foreach (var item in data)
            {
                res.Add(int.Parse(item.Date), item.Money);
            }
            var max = TimeHelper.WeekOfYear(now);
            for (int i = 1; i <= 53; i++)
            {
                if (now.Month < 6)
                {
                    res.TryAdd(startAt.Year * 100 + i, 0);
                }
                if (i <= max)
                {
                    res.TryAdd(now.Year * 100 + i, 0);
                }
            }
        }

        private void GetLogByDay(IDictionary<int, decimal> res, int id)
        {
            var (startAt, endAt) = GetTimeRange(CYCLE_DAY);
            var data = db.Log.Where(i => i.UserId == client.UserId 
                && i.BudgetId == id && i.HappenedAt >= startAt && i.HappenedAt < endAt)
                .GroupBy(i => QueryExtensions.DateFormat(i.HappenedAt, "%d"))
                .Select(i => new {
                    Date = i.Key,
                    Money = i.Sum(j => j.Money)
                }).ToArray();
            var basic = startAt.Year * 10000 + startAt.Month * 100;
            foreach (var item in data)
            {
                res.Add(basic + int.Parse(item.Date), item.Money);
            }
            for (int i = 1; i <= DateTime.DaysInMonth(startAt.Year, startAt.Month); i++)
            {
                res.TryAdd(basic + i, 0);
            }
        }

        private static (DateTime, DateTime) GetTimeRange(byte cycle)
        {
            return GetTimeRange(DateTime.Now, cycle);
        }

        private static (DateTime, DateTime) GetTimeRange(DateTime now, byte cycle)
        {
            if (cycle == CYCLE_DAY)
            {
                var startAt = now.Date;
                return (startAt, startAt.AddDays(1));
            }
            if (cycle == CYCLE_WEEK)
            {
                var startAt = now.Date.AddDays((7 - (int)now.DayOfWeek) % 7 - 7 + 1);
                return (startAt, startAt.AddDays(7));
            }
            if (cycle == CYCLE_MONTH)
            {
                var startAt = new DateTime(now.Year, now.Month, 1);
                return (startAt, startAt.AddMonths(1));
            }
            else
            {
                var startAt = new DateTime(now.Year, 1, 1);
                return (startAt, startAt.AddYears(1));
            }
        }
    }
}
