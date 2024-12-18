using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class ActionLogProvider(
        IDatabase db, 
        string prefix,
        IClientContext environment) : IMigrationProvider
    {
        private readonly string _tableName = prefix + "_log";
        public void Migration(IMigration migration)
        {
            migration.Append(_tableName, table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.Uint("action");
                table.String("ip", 120).Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }
        /// <summary>
        /// 切换记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns>{0: 取消，1: 更新为，2：新增}</returns>
        public byte ToggleLog(byte type, byte action, int id)
        {
            return ToggleLog(type, action, id, [action]);
        }

        /// <summary>
        /// 切换记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <param name="searchAction"></param>
        /// <returns></returns>
        public byte ToggleLog(byte type, byte action, int id, IList<byte> searchAction)
        {
            if (environment.UserId == 0)
            {
                return 0;
            }
            var sql = new Sql();
            sql.Select("*").From(_tableName);
            sql.Where("item_id=@0 AND item_type=@1 AND user_id=@2", id, type, environment.UserId);
            sql.WhereIn("action", searchAction.Select(i => (int)i).ToArray());
            var log = db.First<ActionLog>(sql);
            if (log == null)
            {
                log = new ActionLog
                {
                    UserId = environment.UserId,
                    ItemId = id,
                    ItemType = type,
                    Action = action,
                    CreatedAt = TimeHelper.TimestampNow()
                };
                db.Insert(_tableName, "id", log);
                return 2;
            }
            if (log.Action == action)
            {
                db.Delete(log);
                return 0;
            }
            log.Action = action;
            log.CreatedAt = TimeHelper.TimestampNow();
            db.Update(_tableName, "id", log);
            return 1;
        }


        public byte? GetAction(byte type, int id, IList<byte>? onlyAction = null)
        {
            if (environment.UserId == 0)
            {
                return null;
            }
            var sql = new Sql();
            sql.Select("action").From(_tableName);
            sql.Where("item_id=@0 AND item_type=@1 AND user_id=@2", id, type, environment.UserId);
            if (onlyAction is not null)
            {
                sql.WhereIn("action", onlyAction.Select(i => (int)i).ToArray());
            }
            var log = db.First<ActionLog>(sql);
            return log?.Action;
        }

        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Count(byte type, byte action, int id)
        {
            return db.FindCount(_tableName, "item_id=@0 AND item_type=@1 AND action=@2",
                id, type, action);
        }

        public bool Has(byte type, int id, byte action = 0)
        {
            if (environment.UserId == 0)
            {
                return false;
            }
            return db.FindCount(_tableName, "item_id=@0 AND item_type=@1 AND user_id=@2 AND action=@3",
                id, type, environment.UserId, action) > 0;
        }

        public void Insert(ActionLog data)
        {
            if (string.IsNullOrWhiteSpace(data.Ip))
            {
                data.Ip = environment.Ip;
            }
            if (data.CreatedAt == 0)
            {
                data.CreatedAt = TimeHelper.TimestampNow();
            }
            if (data.UserId == 0)
            {
                data.UserId = environment.UserId;
            }
            var id = db.Insert(_tableName, data);
            if (id == 0)
            {
                throw new Exception("insert log error");
            }
        }

        public void Update(int id, ActionLog data)
        {
            data.Id = id;
            db.TryUpdate(_tableName, data);
        }

        public void Update(string set, string where, params object[] args)
        {
            db.Execute(string.Format("UPDATE {0} SET {1} WHERE {2}",
                    db.DatabaseType.EscapeTableName(_tableName),
                    set, where
                ),
                args);
        }

        public void Remove(int id)
        {
            db.Delete(_tableName, id);
        }

    }
}
