using NetDream.Shared.Helpers;
using NetDream.Shared.Migrations;
using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Extensions
{
    public static class Database
    {
        public static Sql From<T>(this Sql sql, IDatabase db)
            where T : class
        {
            return sql.From(db.DatabaseType.EscapeTableName(ModelHelper.TableName<T>()));
        }
        public static Sql WhereIn(this Sql sql, string key, params int[] args)
        {
            if (args.Length == 0)
            {
                return sql;
            }
            return sql.Where($"{key} IN ({string.Join(',', args)})");
        }

        public static Sql WhereNotIn(this Sql sql, string key, params int[] args)
        {
            if (args.Length == 0)
            {
                return sql;
            }
            return sql.Where($"{key} NOT IN ({string.Join(',', args)})");
        }

        public static Sql WhereIn(this Sql sql, string key, params string[] args)
        {
            if (args.Length == 0)
            {
                return sql;
            }
            var keys = new List<string>();
            var rawValues = new List<object>();
            var index = 0;
            foreach (var item in args)
            {
                keys.Add($"@{index++}");
                rawValues.Add(item);
            }
            return sql.Where($"{key} IN ({string.Join(',', keys)})", [.. rawValues]);
        }

        public static Sql Limit(this Sql sql, long count)
        {
            return sql.Append("LIMIT " + count);
        }

        public static Sql Limit(this Sql sql, long offset, long count)
        {
            return sql.Append($"LIMIT {offset},{count}");
        }

        public static IList<T> Pluck<T>(this IDatabase db, Sql sql, string key)
        {
            sql.Select(key);
            return db.Pluck<T>(sql);
        }

        public static IList<T> Pluck<T>(this IDatabase db, Sql sql)
        {
            var data = new List<T>();
            foreach (var item in db.Query<Dictionary<string, T>>(sql))
            {
                data.Add(item.ElementAt(0).Value);
            }
            return data;
        }

        public static T? FindFirst<T>(this IDatabase db, string select, string where, params object[] args)
            where T : class
        {
            return db.Single<T>(string.Format("SELECT {0} FROM {1} WHERE {2}", 
                    select,
                    db.DatabaseType.EscapeTableName(ModelHelper.TableName<T>()),
                    where
                ), args);
        }

        public static R FindScalar<R, T>(this IDatabase db, string select, string where, params object[] args)
            where T : class
            where R : notnull
        {
            var sql = string.Format("SELECT {0} FROM {1}",
                    select,
                    db.DatabaseType.EscapeTableName(ModelHelper.TableName<T>())
                );
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql += " WHERE " + where;
            }
            return db.ExecuteScalar<R>(sql, args);
        }

        public static R FindCount<R, T>(this IDatabase db, string column, string where, params object[] args)
            where T : class
            where R : notnull
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                column = "*";
            } else if (column != "*" && !column.Contains(' '))
            {
                column = db.DatabaseType.EscapeSqlIdentifier(column);
            }
            return db.FindScalar<R, T>($"COUNT({column}) as count", where, args);
        }

        public static R FindCount<R, T>(this IDatabase db, string where, params object[] args)
            where T : class
            where R : notnull
        {
            return db.FindCount<R, T>("*", where, args);
        }

        public static int FindCount<T>(this IDatabase db, string where, params object[] args)
            where T : class
        {
            return db.FindCount<int, T>(where, args);
        }

        public static IDictionary<T, K> Pluck<T, K>(this IDatabase db, Sql sql, string key, string valueKey, bool autoSelect = true)
            where T : notnull
        {
            if (autoSelect)
            {
                sql.Select($"{key},{valueKey}");
            }
            return db.Dictionary<T, K>(sql);
        }

        public static void InsertBatch<T>(this IDatabase db, IEnumerable<Dictionary<string, object>> items)
            where T : class
        {
            var tableName = ModelHelper.TableName(typeof(T));
            var sql = new Sql();
            foreach (var item in items)
            {
                sql.Insert(db, tableName, item);
            }
            db.Execute(sql);
        }

        public static Sql Insert(this Sql sql, IDatabase db, string tableName, Dictionary<string, object> data)
        {
            return sql.Append(
                    string.Format(
                        "INSERT INTO {0}({1}) VALUES({2})",
                        db.DatabaseType.EscapeTableName(tableName),
                        string.Join(',', data.Keys.Select(i => db.DatabaseType.EscapeSqlIdentifier(i))),
                        string.Join(',', data.Values.Select((_, j) => $"@{j}"))
                    ),
                    [.. data.Values]
            );
        }

        public static void Insert<T>(this IDatabase db, Dictionary<string, object> data)
        {
            var sql = new Sql();
            sql.Insert(db, ModelHelper.TableName(typeof(T)), data);
            db.Execute(sql);
        }

        public static void UpdateById<T>(this IDatabase db, int id, Dictionary<string, object> items)
        {
            db.Update<T>(new Sql().Where("id=@0", id), items);
        }
        public static void Update<T>(this IDatabase db, Sql whereSql, Dictionary<string, object> items)
        {
            var builder = new Sql();
            var keys = new List<string>();
            var rawValues = new List<object>();
            builder.Append(
                string.Format("UPDATE {0} SET {1}",
                    db.DatabaseType.EscapeTableName(ModelHelper.TableName(typeof(T))),
                    string.Join(',', items.Select((item, i) =>
                        string.Format("{0}=@{1}",
                        db.DatabaseType.EscapeSqlIdentifier(item.Key), i)
                    ))
                ),
                [..items.Values]);
            builder.Append(whereSql);
            db.Execute(builder);
        }

        public static void UpdateWhere<T>(this IDatabase db, string set, string where, params object[] args)
        {
            db.Execute(string.Format("UPDATE {0} SET {1} WHERE {2}",
                    db.DatabaseType.EscapeTableName(ModelHelper.TableName(typeof(T))),
                    set, where
                ),
                args);
        }
        /// <summary>
        /// 自动添加时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TrySave<T>(this IDatabase db, T data) 
            where T : class
        {
            var type = typeof(T);
            var now = TimeHelper.TimestampNow();
            var field = type.GetProperty(MigrationTable.COLUMN_UPDATED_AT);
            field?.SetValue(data, now);
            field = type.GetProperty("Id");
            if (field is not null && (int)field.GetValue(data) == 0)
            {
                field = type.GetProperty(MigrationTable.COLUMN_CREATED_AT);
                field?.SetValue(data, now);
            }
            db.Save(data);
            return true;
        }

        public static void DeleteById<T>(this IDatabase db, params int[] items)
        {
            db.DeleteById<T>("id", items);
        }
        public static void DeleteById<T>(this IDatabase db, string columnKey, params int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            if (items.Length == 1)
            {
                db.Delete<T>($"WHERE ${columnKey}=@0", items[0]);
                return;
            }
            db.Delete<T>($"WHERE ${columnKey} IN ({string.Join(',', items)})");
        }
    }
}
