using NetDream.Shared.Helpers;
using NPoco;

namespace NetDream.Shared.Providers
{
    public static class Extension
    {
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

        public static int Update(this IDatabase db, string tableName, object data)
        {
            return db.Update(tableName, "id", data);
        }

        public static void UpdateWhere(this IDatabase db, 
            string tableName, string where, 
            IDictionary<string, object> items)
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
            builder.Where(where);
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
            sql.From(tableName).Where(where, args);
            return db.Delete(sql);
        }
    }
}
