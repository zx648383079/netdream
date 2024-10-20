using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NPoco;

namespace NetDream.Shared.Providers
{
    public static class Extension
    {
        public static Sql DeleteFrom(this Sql sql, string tableName, IDatabase db)
        {
            return sql.Append("DELETE").From(db.DatabaseType.EscapeTableName(tableName));
        }
        public static int FindCount(this IDatabase db, string tableName, string where, params object[] args)
        {
            var sql = new Sql();
            sql.Select("COUNT(*) AS count").From(tableName);
            sql.Where(where, args);
            return db.ExecuteScalar<int>(sql);
        }
        public static T? FindById<T>(this IDatabase db, string tableName, int id)
        {
            var sql = new Sql();
            sql.Select("*").From(tableName).Where("id=@0", id);
            return db.First<T>(sql);
        }

        public static R FindScalar<R>(this IDatabase db, string tableName, string select, string where, params object[] args)
            where R : notnull
        {
            var sql = string.Format("SELECT {0} FROM {1}",
                    select,
                    db.DatabaseType.EscapeTableName(tableName)
                );
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql += " WHERE " + where;
            }
            return db.ExecuteScalar<R>(sql, args);
        }

        public static T? FindFirst<T>(this IDatabase db, string tableName, string where, params object[] args)
        {
            var sql = new Sql();
            sql.Select("*").From(tableName).Where(where, args);
            return db.Single<T>(sql);
        }

        public static int Insert(this IDatabase db, string tableName, object data)
        {
            var id = db.Insert(tableName, "id", data);
            return Convert.ToInt32(id);
        }

        public static void InsertBatch(this IDatabase db, string tableName, IEnumerable<Dictionary<string, object>> items)
        {
            var sql = new Sql();
            foreach (var item in items)
            {
                sql.Insert(db, tableName, item);
            }
            db.Execute(sql);
        }

        public static int TryUpdate(this IDatabase db, string tableName, object data)
        {
            return db.Update(tableName, "id", data);
        }

        public static void UpdateWhere(this IDatabase db, 
            string tableName, string where, 
            IDictionary<string, object> items, params object[] args)
        {
            var builder = new Sql();
            var keys = new List<string>();
            var rawValues = new List<object>();
            builder.Append(
                string.Format("UPDATE {0} SET {1}",
                    db.DatabaseType.EscapeTableName(tableName),
                    string.Join(',', items.Select((item, i) =>
                        string.Format("{0}=@{1}",
                        db.DatabaseType.EscapeSqlIdentifier(item.Key), i)
                    ))
                ),
                [.. items.Values]);
            builder.Where(where, args);
            db.Execute(builder);
        }

        public static void UpdateById(this IDatabase db, string tableName, 
            int id, IDictionary<string, object> items)
        {
            db.UpdateWhere(tableName, "id=" + id, items);
        }

        public static int Delete(this IDatabase db, string tableName, int id)
        {
            return db.Delete(tableName, "id", id);
        }

        public static int DeleteWhere(this IDatabase db, string tableName, string where, params object[] args)
        {
            var sql = new Sql();
            sql.DeleteFrom(tableName, db).Where(where, args);
            return db.Execute(sql);
        }
        public static int DeleteWhere(this IDatabase db, Sql sql)
        {
            if (!sql.SQL.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                sql = new Sql().Append("DELETE").Append(sql);
            }
            return db.Execute(sql);
        }

        /// <summary>
        /// 更新自增字段
        /// </summary>
        /// <param name="cb">(oldId, newId)</param>
        /// <param name="key"></param>
        public static void RefreshPk<T>(this IDatabase db, Action<int, int> cb, string key = "id")
            where T : class
        {
            var tableName = db.DatabaseType.EscapeTableName(ModelHelper.TableName<T>());
            var sql = new Sql();
            sql.Select(key).From(tableName)
                .OrderBy($"{key} asc");
            var data = db.Pluck<int>(sql);
            var i = 1;
            foreach (var id in data)
            {
                if (id == i)
                {
                    i++;
                    continue;
                }
                db.UpdateWhere<T>("id=@0", "id=@1", i, id);
                cb.Invoke(id, i);
                i++;
            }
            db.Execute($"ALTER TABLE {tableName} AUTO_INCREMENT = {i};");
        }
    }
}
