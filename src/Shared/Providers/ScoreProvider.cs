using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Providers.Models;
using System;
using System.Linq;


namespace NetDream.Shared.Providers
{
    public class ScoreProvider(ISoreContext db, IClientContext client, IUserRepository userStore)
    {

        public IPage<ScoreListItem> Search(ScoreQueryForm form)
        {
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, [
                "id",
                "created_at",
                "score"
            ]);
            var items = db.ScoreLogs.When(form.User > 0, i => i.UserId == form.User)
                .When(form.ItemId > 0, i => i.ItemId == form.ItemId && i.ItemType == form.ItemType)
                .When(form.FromId > 0, i => i.FromId == form.FromId && i.FromType == form.FromType)
                .OrderBy<ScoreLogEntity, int>(sort, order)
                .ToPage(form, i => i.Select(j => new ScoreListItem()
                {
                    Id = j.Id,
                    CreatedAt = j.CreatedAt,
                    FromId = j.FromId,
                    FromType = j.FromType,
                    ItemId = j.ItemId,
                    ItemType = j.ItemType,
                    UserId = j.UserId,
                }));
            userStore.Include(items.Items);
            return items;
        }

        public void Remove(int id)
        {
            db.ScoreLogs.Where(i => i.Id == id).ExecuteDelete();
        }

        public IOperationResult<int> Insert(ScoreLogEntity data)
        {
            db.ScoreLogs.Add(data);
            db.SaveChanges();
            if (data.Id == 0)
            {
                return OperationResult<int>.Fail("insert log error");
            }
            return OperationResult.Ok(data.Id);
        }

        public void Update(int id, ScoreLogEntity data)
        {
            data.Id = id;
            db.ScoreLogs.Update(data);
            db.SaveChanges();
        }

        public IOperationResult<ScoreLogEntity> Get(int id)
        {
            var model = db.ScoreLogs.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<ScoreLogEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
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
            && i.UserId == client.UserId && i.FromType == fromType 
            && i.FromId == fromId).Any();
        }

        public IOperationResult<ScoreLogEntity> Save(ScoreLogEntity data)
        {
            data.UserId = client.UserId;
            data.CreatedAt = client.Now;
            db.ScoreLogs.Add(data);
            db.SaveChanges();
            if (data.Id == 0)
            {
                return OperationResult<ScoreLogEntity>.Fail("insert log error");
            }
            return OperationResult.Ok(data);
        }

        public IOperationResult<ScoreLogEntity> Add(ScoreForm form)
        {
            return Save(new ScoreLogEntity()
            {
                Score = form.Score,
                ItemId = form.ItemId,
                ItemType = form.ItemType,
                FromId = form.FromId,
                FromType = form.FromType,
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
            if (client.UserId == 0)
            {
                return;
            }
            db.ScoreLogs.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
        }
    }
}
