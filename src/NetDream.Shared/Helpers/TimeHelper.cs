using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetDream.Shared.Helpers
{
    public static class TimeHelper
    {
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(DateTime date, string format)
        {
            return date.ToString(format);
        }

        public static string Format(int timestamp, string format)
        {
            return Format(TimestampTo(timestamp), format);
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Format(DateTime date)
        {
            return Format(date, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化当前时间
        /// </summary>
        /// <returns></returns>
        public static string Format()
        {
            return Format(DateTime.Now);
        }

        /// <summary>
        /// 格式化时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string Format(int timestamp)
        {
            return Format(TimestampTo(timestamp));
        }

        /// <summary>
        /// 从时间戳转时间
        /// </summary>
        /// <param name="timestamp">秒</param>
        /// <returns></returns>
        public static DateTime TimestampTo(int timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }

        public static int TimestampFrom(string time)
        {
            if (DateTime.TryParse(time, out var res))
            {
                return TimestampFrom(res);
            }
            return 0;
        }
        /// <summary>
        /// 从时间转时间戳，精确到秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int TimestampFrom(DateTime time)
        {
            return (int)new DateTimeOffset(time).ToUnixTimeSeconds();
        }

        public static int TimestampNow()
        {
            return TimestampFrom(DateTime.Now);
        }

        /// <summary>
        /// 格式化多久之前
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatAgo(DateTime date)
        {
            var now = DateTime.Now;
            var diff = now - date;
            if (diff.TotalSeconds < 1)
            {
                return "刚刚";
            }
            if (diff.TotalSeconds < 60)
            {
                return $"{diff.TotalSeconds}秒前";
            }
            if (diff.TotalMinutes < 60)
            {
                return $"{diff.TotalSeconds}分钟前";
            }
            if (diff.TotalMinutes < 60)
            {
                return $"{diff.TotalMinutes}分钟前";
            }
            if (diff.TotalHours < 24)
            {
                return date.Day == now.Day ? $"{diff.TotalHours}小时前" : date.ToString("MM-dd");
            }
            if (date.Year == now.Year)
            {
                return date.ToString("MM-dd");
            }
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化多久之前
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string FormatAgo(int timestamp)
        {
            return FormatAgo(TimestampTo(timestamp));
        }

        public static int Millisecond()
        {
            return DateTime.Now.Millisecond;
        }


        /// <summary>
        /// 当月最大天数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int MonthMaxDay(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        public static int WeekOfYear(DateTime date)
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static (DateTime, DateTime) DayRange(DateTime date)
        {
            var begin = date.Date;
            var end = date.AddDays(1);
            return (begin, end);
        }

        public static (DateTime, DateTime) WeekRange(DateTime date)
        {
            var begin = date.AddDays((7 - (int)date.DayOfWeek) % 7 - 7 + 1);
            var end = begin.AddDays(7);
            return (begin, end);
        }

        

        /// <summary>
        /// 获取单月起始时间结束时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static (DateTime, DateTime) MonthRange(DateTime date)
        {
            var begin = new DateTime(date.Year, date.Month, 1);
            var end = begin.AddMonths(1);
            return (begin, end);
        }

        public static (DateTime, DateTime) YearRange(DateTime date)
        {
            var begin = new DateTime(date.Year, 1, 1);
            var end = begin.AddYears(1);
            return (begin, end);
        }
        public static string[] RangeDate(int begin, int end)
        {
            return RangeDate(TimestampTo(begin), TimestampTo(end));
        }
        public static string[] RangeDate(DateTime begin, DateTime end)
        {
            var items = new List<string>();
            while (begin <= end)
            {
                items.Add(begin.ToString("yyyy-MM-dd"));
                begin.AddDays(1);
            }
            return [.. items];
        }
    }
}
