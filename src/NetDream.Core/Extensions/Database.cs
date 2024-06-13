﻿using NetDream.Core.Helpers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Core.Extensions
{
    public static class Database
    {
        public static Sql WhereIn(this Sql sql, string key, params int[] args)
        {
            if (args.Length == 0)
            {
                return sql;
            }
            return sql.Where($"{key} IN ({string.Join(',', args)})");
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

        private static Sql Insert(this Sql sql, IDatabase db, string tableName, Dictionary<string, object> data)
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

        public static void Update<T>(this IDatabase db, Sql sql, Dictionary<string, object> items)
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
            builder.Append(sql);
            db.Execute(builder);
        }
    }
}
