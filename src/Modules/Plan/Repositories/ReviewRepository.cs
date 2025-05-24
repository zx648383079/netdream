using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class ReviewRepository(PlanContext db, IClientContext client)
    {


        public ReviewListItem[] Statistics(string start_at, string end_at, 
            bool ignoreEmpty = false)
        {
            var start = TimeHelper.TimestampFrom(start_at);
            var end = TimeHelper.TimestampFrom(end_at);
            var data = db.Days.Where(i => 
            i.CreatedAt >= start 
            && i.CreatedAt <= end && i.UserId == client.UserId)
                .OrderBy(i => i.Today).ToArray();
            var days = TimeHelper.RangeDate(start, end);
            return FormatStatistics(days, data, ignoreEmpty);
        }

        public ReviewListItem DayStatistics(string day)
        {
            var dayFormat = DateOnly.Parse(day);
            var data = db.Days
                .Where(i => i.Today == dayFormat && i.UserId == client.UserId)
                .OrderBy(i => i.Today).ToArray();
            return FormatStatistics([day], data)[0];
        }

        /**
         * @param int start_at
         * @param int end_at
         * @param bool isAll
         * @return Page<LogPageModel>|LogPageModel[]
         * @throws Exception
         */
        public IPage<LogListItem> LogList(QueryForm form, int start_at, int end_at)
        {
            var res = db.Logs
                .Where(i => i.CreatedAt >= start_at && i.EndAt <= end_at && i.UserId == client.UserId)
                .OrderBy(i => i.CreatedAt)
                .ToPage<LogEntity, LogListItem>(form);
            TaskRepository.Include(db, res.Items);
            return res;
        }
        public LogListItem[] LogList(int start_at, int end_at)
        {
            var res = db.Logs
                .Where(i => i.CreatedAt >= start_at && i.EndAt <= end_at && i.UserId == client.UserId)
                .OrderBy(i => i.CreatedAt)
                .ToArray().CopyTo<LogEntity, LogListItem>();
            TaskRepository.Include(db, res);
            return res;
        }

        /**
         * @param ignoreEmpty
         * @param array days
         * @param data
         * @return array
         */
        public ReviewListItem[] FormatStatistics(string[] days, DayEntity[] data, bool ignoreEmpty = false)
        {
            var day_list = days.Select(i => new ReviewListItem()
            {
                Day = i,
                Week = DateTime.Parse(i).DayOfWeek.ToString(),
            }).ToDictionary(i => i.Day);
            foreach (var item in data)
            {
                if (!day_list.TryGetValue(item.Today.ToString("yyyyMMdd"), out var log))
                {
                    continue;
                }
                log.Amount += item.Amount + item.SuccessAmount;
                log.SuccessAmount += item.SuccessAmount;
                log.PauseAmount += item.PauseAmount;
                log.FailureAmount += item.FailureAmount;
                if (item.Amount < 1)
                {
                    log.CompleteAmount ++;
                }
                
                foreach (var it in db.Logs.Where(i => i.DayId == item.Id))
                {
                    var time = TaskRepository.GetSpentTime(it, client.Now);
                    log.TotalTime += time;
                    if (it.Status == TaskRepository.LOG_STATUS_FINISH)
                    {
                        log.ValidTime += time;
                    }
                }
            }
            if (ignoreEmpty)
            {
                return day_list.Values.Where(i => i.Amount > 0).OrderBy(i => i.Day).ToArray();
            }
            return day_list.Values.OrderBy(i => i.Day).ToArray();
        }

       
    }
}
