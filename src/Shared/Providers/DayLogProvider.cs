using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NetDream.Shared.Providers
{
    /// <summary>
    /// 按天统计的记录
    /// </summary>
    /// <param name="db"></param>
    /// <param name="prefix"></param>
    public class DayLogProvider(IDatabase db, 
        string prefix, IClientEnvironment environment) : IMigrationProvider
    {
        private readonly string _logTableName = prefix + "_log";
        private readonly string _dayTableName = prefix + "_log_day";
        public void Migration(IMigration migration)
        {
            migration.Append(_logTableName, table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id").Default(0);
                table.Uint("action").Default(0);
                table.String("ip", 120).Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append(_dayTableName, table => {
                table.Id();
                table.String("happen_day", 20);
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("action").Default(0);
                table.Uint("happen_count").Default(0);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }

        public void Add(byte itemType, int itemId, byte action)
        {
            db.Insert(_logTableName, new ActionLog()
            {
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                UserId = environment.UserId,
                CreatedAt = environment.Now
            });
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
            return db.FindCount(_logTableName,
                "item_id=@0 AND item_type=@1 AND action=@2 AND user_id=@3",
                itemId, itemType, action, environment.UserId) > 0;
        }

        protected bool HasCount(byte itemType, int itemId, byte action, string to)
        {
            return db.FindCount(_dayTableName,
                "item_id=@0 AND item_type=@1 AND action=@2 AND happen_day=@3",
                itemId, itemType, action, to) > 0;
        }

        protected void AddCount(byte itemType, int itemId, byte action, 
            string to, int start, int end)
        {
            if (HasCount(itemType, itemId, action, to))
            {
                return;
            }
            var count = db.FindCount(_logTableName,
                "item_id=@0 AND item_type=@1 AND action=@2 AND created_at>=@3 AND created_at<=@4",
                itemId, itemType, action, start, end);
            db.Insert(_dayTableName, new DayActionLog()
            {
                HappenDay = to,
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                HappenCount = count,
                CreatedAt = environment.Now,
            });
            db.DeleteWhere(_logTableName, "item_id=@0 AND item_type=@1 AND action=@2 AND created_at>=@3 AND created_at<=@4",
                itemId, itemType, action, start, end);
        }

        protected void MergeCount(byte itemType, int itemId, 
            byte action, string to, string[] from)
        {
            if (HasCount(itemType, itemId, action, to))
            {
                return;
            }
            var sql = new Sql();
            sql.Select("COUNT(*) AS count")
                .From(_dayTableName)
                .Where("item_id=@0 AND item_type=@1 AND action=@2", itemId, itemType, action)
                .WhereIn("happen_day", from);
            var count = db.ExecuteScalar<int>(sql);
            db.Insert(_dayTableName, new DayActionLog()
            {
                HappenDay = to,
                ItemType = itemType,
                ItemId = itemId,
                Action = action,
                HappenCount = count,
                CreatedAt = environment.Now,
            });
            sql = new Sql();
            sql.DeleteFrom(_dayTableName, db)
                .Where("item_id=@0 AND item_type=@1 AND action=@2", itemId, itemType, action)
                .WhereIn("happen_day", from);
            db.DeleteWhere(sql);
        }

        public int Count(byte itemType, int itemId, byte action)
        {
            return db.FindCount(_logTableName, 
                "item_id=@0 AND item_type=@1 AND action=@2", 
                itemId, itemType, action)
                + db.FindScalar<int>(_dayTableName, "SUM(happen_count) AS count", 
                "item_id=@0 AND item_type=@1 AND action=@2", 
                itemId, itemType, action);
        }

        public int DayCount(byte itemType, int itemId, byte action, string day)
        {
            var sql = new Sql();
            sql.Select("happen_count").From(_dayTableName)
                .Where("item_type=@0 AND action=@1 AND happen_day=@2", 
                itemType, action, day);
            if (itemId > 0)
            {
                sql.Where("item_id=@0", itemId);
            }
            var log = db.Single<DayActionLog>(sql);
            if (log is not null)
            {
                return log.HappenCount;
            }
            var start = TimeHelper.TimestampFrom(DateTime.Parse(day + " 00:00:00"));
            sql = new Sql();
            sql.Select("COUNT(*) AS count").From(_logTableName)
                .Where("item_type=@0 AND action=@1 AND created_at>=@2 AND created_at<@3", 
                itemType, action, start, start + 86400);
            if (itemId > 0)
            {
                sql.Where("item_id=@0", itemId);
            }
            return db.ExecuteScalar<int>(sql);
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
            var sql = new Sql();
            sql.Select("id", MigrationTable.COLUMN_CREATED_AT).From(_logTableName)
                .Where("item_type=@0 AND action=@1 AND item_id=@2 AND created_at>=@3",
                itemType, action, itemId, start);
            var data = db.Fetch<ActionLog>();
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
                        it.Value ++;
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
            var sql = new Sql();
            sql.Select("happen_day", "happen_count")
                .From(_dayTableName)
                .Where("item_type=@0 AND item_id=@1 AND action=@2",
                itemType, itemId, action)
                .WhereIn("happen_day", items.Select(i => i.Name).ToArray())
                .OrderBy("count DESC");
            var data = db.Fetch<DayActionLog>(sql);
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
            var sql = new Sql();
            sql.Select("happen_day", "happen_count")
                .From(_dayTableName)
                .Where("item_type=@0 AND item_id=@1 AND action=@2",
                itemType, itemId, action)
                .WhereIn("happen_day", items.Select(i => i.Name).ToArray())
                .OrderBy("count DESC");
            var data = db.Fetch<DayActionLog>(sql);
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

        public IList<LogCount> SortByDay(byte itemType, byte action)
        {
            var sql = new Sql();
            sql.Select("item_id", "COUNT(*) as count")
                .From(_logTableName)
                .Where("item_type=@0 AND action=@1 AND created_at>=@2",
                itemType, action, TimeHelper.TimestampFrom(DateTime.Today))
                .GroupBy("item_id")
                .OrderBy("count DESC");
            return db.Fetch<LogCount>(sql);
        }

        public IList<LogCount> SortByWeek(byte itemType, byte action)
        {
            var (start, end) = TimeHelper.WeekRange(DateTime.Now);
            var dayItems = TimeHelper.RangeDate(start, end);
            var sql = new Sql();
            sql.Select("item_id", "SUM(happen_count) as count")
                .From(_dayTableName)
                .Where("item_type=@0 AND action=@1",
                itemType, action)
                .WhereIn("happen_day", dayItems)
                .OrderBy("count DESC");
            return db.Fetch<LogCount>(sql);
        }

        public IList<LogCount> SortByMonth(byte itemType, byte action)
        {
            var (start, end) = TimeHelper.MonthRange(DateTime.Now);
            var dayItems = TimeHelper.RangeDate(start, end);
            var sql = new Sql();
            sql.Select("item_id", "SUM(happen_count) as count")
                .From(_dayTableName)
                .Where("item_type=@0 AND action=@1",
                itemType, action)
                .WhereIn("happen_day", dayItems)
                .OrderBy("count DESC");
            return db.Fetch<LogCount>(sql);
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
            MergeCount(itemType, itemId, action, month, [..dayItems]);
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
