using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace NetDream.Shared.Providers
{
    public class ScoreProvider(IDatabase db, 
        string prefix, IClientContext environment) : IMigrationProvider
    {
        private readonly string _tableName = prefix + "_score";
        public void Migration(IMigration migration)
        {
            migration.Append(_tableName, table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.Uint("score", 1).Default(6);
                table.Uint("from_type", 1).Default(0);
                table.Uint("from_id").Default(0);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }

        public Page<ScoreLog> Search(byte itemType = 0, int itemId = 0, 
            int user = 0, byte fromType = 0, int fromId = 0, string sort = "id", string order = "desc")
        {
            //list(sort, order) = SearchModel.CheckSortOrder(sort, order, [
            //    "id",
            //MigrationTable.COLUMN_CREATED_AT,
            //"score"
            //]);
            //page = Query().When(user > 0, use(user)(object query) {
            //    query.Where("user_id", user);
            //})
            //.When(itemId > 0, use(itemId, itemType)(object query) {
            //    query.Where("item_id", itemId).Where("item_type", itemType);
            //})
            //.When(fromId > 0, use(fromId, fromType)(object query) {
            //    query.Where("from_id", fromId).Where("from_type", fromType);
            //}).OrderBy(sort, order).Page();
            //data = page.GetPage();
            //if (empty(data))
            //{
            //    return page;
            //}
            //data = Relation.Create(data, [
            //    "user" => Relation.Make(UserSimpleModel.Query(), "user_id", "id")
            //]);
            //page.SetPage(data);
            //return page;
            return null;
        }

        public void Remove(int id)
        {
            db.Delete(_tableName, id);
        }

        public int Insert(ScoreLog data)
        {
            data.Id = db.Insert(_tableName, data);
            if (data.Id == 0)
            {
                throw new Exception("insert log error");
            }
            return data.Id;
        }

        public void Update(int id, ScoreLog data)
        {
            data.Id = id;
            db.TryUpdate(_tableName, data);
        }

        public ScoreLog? Get(int id)
        {
            return db.FindById<ScoreLog>(_tableName, id);
        }

        /// <summary>
        /// 是否已经评价了
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="fromType"></param>
        /// <param name="fromId"></param>
        /// <returns></returns>
        public bool Has(int itemId, byte itemType = 0, byte fromType = 0, 
            int fromId = 0)
        {
            return db.FindCount(_tableName,
                "item_id=@0 AND item_type=@1 AND user_id=@2 AND from_type=@3 AND from_id=@4",
                itemId, itemType, environment.UserId, fromType, fromId) > 0;
        }

        public ScoreLog Save(ScoreLog data)
        {
            data.UserId = environment.UserId;
            data.CreatedAt = environment.Now;
            data.Id = Insert(data);
            // data["user"] = UserSimpleModel.ConverterFrom(auth().User());
            return data;
        }

        public ScoreLog Add(byte score, int item_id, byte item_type = 0, byte from_type = 0, int from_id = 0)
        {
            return Save(new ScoreLog()
            {
                Score = score,
                ItemId = item_id,
                ItemType = item_type,
                FromId = from_id,
                FromType = from_type,
            });
        }

        /// <summary>
        /// 获取平均值
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public float Avg(int itemId, byte itemType = 0)
        {
            var sql = new Sql();
            sql.Select("AVG(score) as s")
                .From(_tableName)
                .Where("item_id=@0 AND item_type=@1", itemId, itemType);
            return db.ExecuteScalar<float>(sql);
        }

        /**
         * 获取评分的统计信息
         * @param int itemId
         * @param int itemType
         * @return array{total: int, good: int, middle: int, bad: int, avg: float, favorable_rate: float, tags: array}
         */
        public ScoreSubtotal Count(int itemId, byte itemType = 0)
        {
            var sql = new Sql();
            sql.Select("score", "COUNT(*) AS count")
                .From(_tableName)
                .Where("item_id=@0 AND item_type=@1", itemId, itemType)
                .GroupBy("score");
            var data = db.Fetch<ScoreCount>(sql);
            var args = new ScoreSubtotal();
            var total = 0;
            foreach (var item in data)
            {
                total += item.Count * item.Score;
                args.Total += item.Count;
                if (item.Score > 7)
                {
                    args.Good += item.Count;
                    continue;
                }
                if (item.Score < 3)
                {
                    args.Bad += item.Count;
                    continue;
                }
                args.Middle += item.Count;
            }
            args.Avg = args.Total > 0 ? total / args.Total : 10;
            args.FavorableRate = args.Total > 0 ?
                (float)Math.Ceiling((double)(args.Good * 100 / args.Total)) : 100;
            return args;
        }

        public void RemoveBySelf(int id)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.DeleteWhere(_tableName, "id=@0 AND user_id=@1", id, environment.UserId);
        }
    }
}
