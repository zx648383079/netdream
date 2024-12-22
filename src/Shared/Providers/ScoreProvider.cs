using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Models;
using System;
using System.Linq;


namespace NetDream.Shared.Providers
{
    public class ScoreProvider(ISoreContext db, IClientContext environment)
    {

        public IPage<ScoreLogEntity> Search(byte itemType = 0, int itemId = 0, 
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
            db.ScoreLogs.Where(i => i.Id == id).ExecuteDelete();
        }

        public int Insert(ScoreLogEntity data)
        {
            db.ScoreLogs.Add(data);
            db.SaveChanges();
            if (data.Id == 0)
            {
                throw new Exception("insert log error");
            }
            return data.Id;
        }

        public void Update(int id, ScoreLogEntity data)
        {
            data.Id = id;
            db.ScoreLogs.Update(data);
            db.SaveChanges();
        }

        public ScoreLogEntity? Get(int id)
        {
            return db.ScoreLogs.Where(i => i.Id == id).Single();
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
            return db.ScoreLogs.Where(i => i.ItemType == itemType && i.ItemId == itemId 
            && i.UserId == environment.UserId && i.FromType == fromType 
            && i.FromId == fromId).Any();
        }

        public ScoreLogEntity Save(ScoreLogEntity data)
        {
            data.UserId = environment.UserId;
            data.CreatedAt = environment.Now;
            data.Id = Insert(data);
            // data["user"] = UserSimpleModel.ConverterFrom(auth().User());
            return data;
        }

        public ScoreLogEntity Add(byte score, int item_id, byte item_type = 0, byte from_type = 0, int from_id = 0)
        {
            return Save(new ScoreLogEntity()
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
            return (float)db.ScoreLogs.Where(i => i.ItemType == itemType && i.ItemId == itemId)
                .Average(i => i.Score);
        }

        /**
         * 获取评分的统计信息
         * @param int itemId
         * @param int itemType
         * @return array{total: int, good: int, middle: int, bad: int, avg: float, favorable_rate: float, tags: array}
         */
        public ScoreSubtotal Count(int itemId, byte itemType = 0)
        {
            var data = db.ScoreLogs.Where(i => i.ItemId == itemId && i.ItemType == itemType)
                .GroupBy(i => i.Score)
                .Select(i => new ScoreCount() {
                    Score = i.Key,
                    Count = i.Count()
                }).ToArray();
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
            db.ScoreLogs.Where(i => i.Id == id && i.UserId == environment.UserId).ExecuteDelete();
        }
    }
}
