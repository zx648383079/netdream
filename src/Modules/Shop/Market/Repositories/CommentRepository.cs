using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
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

        /// <summary>
        /// 获取好评率
        /// </summary>
        /// <param name="item_id"></param>
        /// <param name="item_type"></param>
        /// <returns></returns>
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
