using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using System;
using System.Linq;
using CommentListItem = NetDream.Modules.Shop.Market.Models.CommentListItem;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CommentRepository(ShopContext db, 
        IClientContext client, 
        IUserRepository userStore)
    {
        public IPage<CommentListItem> GetList(int item_id, QueryForm form, int item_type = 0)
        {
            var res = db.Comments.Search(form.Keywords, "content")
                .Where(i => i.ItemType == item_type && i.ItemId == item_id)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(res.Items);
            IncludeImage(res.Items);
            return res;
        }

        public IOperationResult<CommentEntity> Create(CommentForm data)
        {
            var model = new CommentEntity()
            {
                ItemId = data.ItemId,
                ItemType = data.ItemType,
                Title = data.Title,
                Content = data.Content,
                Rank = data.Rank,
                UserId = client.UserId,
            };
            db.Comments.Save(model, client.Now);
            db.SaveChanges();
            if (data.Images?.Length > 0)
            {
                foreach (var item in data.Images)
                {
                    db.CommentImages.Add(new CommentImageEntity()
                    {
                        CommentId = model.Id,
                        Image = item,
                        CreatedAt = client.Now,
                        UpdatedAt = client.Now,
                    });
                }
                db.SaveChanges();
            }
            return OperationResult.Ok(model);
        }

        /**
         * 获取好评率
         * @param int item_id
         * @param int item_type
         * @return float %
         */
        public float FavorableRate(int item_id, int item_type = 0)
        {
            var total = db.Comments.Where(i => i.ItemType == item_type
                && i.ItemId == item_id)
                .Count();
            if (total < 1)
            {
                return 100;
            }
            var good = db.Comments.Where(i => i.ItemType == item_type
                && i.ItemId == item_id && i.Rank > 7).Count();
            return good * 100 / total;
        }

        /// <summary>
        /// 获取评论的统计信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public ScoreSubtotal Count(int itemId, int itemType = 0)
        {
            var data = db.Comments.Where(i => i.ItemId == itemId && i.ItemType == itemType)
                .GroupBy(i => i.Rank)
                .Select(i => new ScoreCount()
                {
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

        public CommentListItem[] Recommend(int limit = 6)
        {
            var res = db.Comments.Where(i => i.ItemType == 0)
                .Take(limit).ToArray().CopyTo<CommentEntity, CommentListItem>();
            if (res.Length == 0)
            {
                return res;
            }
            userStore.Include(res);
            IncludeGoods(res);
            IncludeImage(res);
            return res;
        }

        private void IncludeImage(CommentListItem[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.CommentImages.Where(i => idItems.Contains(i.CommentId))
                .ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.Images = data.Where(i => i.CommentId == item.Id).ToArray();
            }
        }

        private void IncludeGoods(CommentListItem[] items)
        {
            var idItems = items.Select(i => i.ItemId).Where(i => i > 0).Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = GoodsRepository.AsList(db.Goods.Where(i => idItems.Contains(i.Id)))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.ItemId > 0 && data.TryGetValue(item.ItemId, out var res))
                {
                    item.Goods = res;
                }
            }
        }
    }
}
