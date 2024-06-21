using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;
using System.Text.Json;

namespace NetDream.Shared.Providers
{
    public class SketchBoxProvider(IDatabase db,
        IClientEnvironment environment,
        byte itemType, int maxUndoCount = 1) : IMigrationProvider
    {
        public const string SKETCH_TABLE = "base_sketch_box";
        public void Migration(IMigration migration)
        {
            migration.Append(SKETCH_TABLE, table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id").Default(0);
                table.Uint("user_id");
                table.MediumText("data");
                table.String("ip", 120).Default(string.Empty);
                table.Timestamps();
            });
        }

        public void Save(object data, int target = 0)
        {
            var sql = new Sql();
            sql.Select("id")
                .From(SKETCH_TABLE)
                .Where("item_type=@0 AND item_id=@1 AND user_id=@2", itemType, target, environment.UserId)
                .OrderBy("updated_at DESC");
            var logList = db.Fetch<SketchLog>(sql);
            if (logList is null || logList.Count < maxUndoCount)
            {
                db.Insert(SKETCH_TABLE, new SketchLog()
                {
                    ItemType = itemType,
                    ItemId = target,
                    Data = JsonSerializer.Serialize(data),
                    UserId = environment.UserId,
                    Ip = environment.Ip,
                    CreatedAt = environment.Now,
                    UpdatedAt = environment.Now,
                });
                return;
            }
            // unset(saveData[MigrationTable.COLUMN_CREATED_AT]);
            db.UpdateById(SKETCH_TABLE, logList.Last().Id, new Dictionary<string, object>()
            {
                {"item_type", itemType },
                { "item_id", target },
                { "data", JsonSerializer.Serialize(data) },
                //{ "user_id", environment.UserId },
                { "ip", environment.Ip },
                { MigrationTable.COLUMN_UPDATED_AT, environment.Now}
            });

            if (logList.Count <= maxUndoCount)
            {
                return;
            }
            var del = logList.Slice(maxUndoCount, logList.Count - maxUndoCount)
                .Select(item => item.Id);
            db.DeleteWhere(SKETCH_TABLE, $"id IN ({string.Join(',', del)})");
        }

        /**
         * 获取保存的全部记录
         * @param int target
         * @return array
         * @throws \Exception
         */
        public IList<SketchLog> Stack(int target = 0)
        {
            var sql = new Sql();
            sql.Select("id", "ip", MigrationTable.COLUMN_UPDATED_AT, MigrationTable.COLUMN_CREATED_AT);
            sql.From(SKETCH_TABLE)
                .Where("item_type=@0 AND item_id=@1 AND user_id=@2", itemType, target, environment.UserId)
                .OrderBy("updated_at DESC");
            return db.Fetch<SketchLog>(sql);
        }

        public T? Get<T>(int target = 0)
        {
            var data = db.FindScalar<string>(SKETCH_TABLE, "data", 
                "item_type=@0 AND item_id=@1 AND user_id=@2 ORDER BY updated_at DESC",
                itemType, target, environment.UserId);
            if (data is null || string.IsNullOrWhiteSpace(data))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public T? GetById<T>(int id)
        {
            var data = db.FindScalar<string>(SKETCH_TABLE, "data", "item_type=@0 AND id=@1 AND user_id=@2", 
                itemType, id, environment.UserId);
            if (data is null || string.IsNullOrWhiteSpace(data))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public void Remove(int target = 0)
        {
            db.DeleteWhere(SKETCH_TABLE, "item_type=@0 AND item_id=@1 AND user_id=@2", 
                itemType, target, environment.UserId);
        }

        public void Clear()
        {
            db.DeleteWhere(SKETCH_TABLE, "item_type=@0", itemType);
        }
    }
}
