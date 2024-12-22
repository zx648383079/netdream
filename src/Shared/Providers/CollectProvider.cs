using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using System;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class CollectProvider(ICollectContext db, IClientContext environment)
    {

        //public Page Search(int itemType = 0, int itemId = 0, int user = 0, string sort = "id", string order = "desc")
        //{
        //    list(sort, order) = SearchModel.CheckSortOrder(sort, order, [
        //        "id",
        //    MigrationTable.COLUMN_CREATED_AT,
        //]);
        //    page = Query().When(user > 0, use(user)(object query) {
        //        query.Where("user_id", user);
        //    })
        //    .When(itemId > 0, use(itemId, itemType)(object query) {
        //        query.Where("item_id", itemId).Where("item_type", itemType);
        //    }).OrderBy(sort, order).Page();
        //    data = page.GetPage();
        //    if (empty(data))
        //    {
        //        return page;
        //    }
        //    data = Relation.Create(data, [
        //        "user" => Relation.Make(UserSimpleModel.Query(), "user_id", "id")
        //    ]);
        //    page.SetPage(data);
        //    return page;
        //}

        public void Remove(int id)
        {
            db.Collects.Where(i => i.Id == id).ExecuteDelete();
        }

        public int Insert(CollectLogEntity data)
        {
            db.Collects.Add(data);
            db.SaveChanges();
            if (data.Id == 0)
            {
                throw new Exception("insert log error");
            }
            return data.Id;
        }

        public void Update(int id, CollectLogEntity data)
        {
            data.Id = id;
            db.Collects.Update(data);
            db.SaveChanges();
        }

        public CollectLogEntity? Get(int id)
        {
            return db.Collects.Where(i => i.Id == id).Single();
        }

        /**
         * 是否已经收藏了
         * @param int itemId
         * @param int itemType
         * @return bool
         * @throws \Exception
         */
        public bool Has(int itemId, int itemType = 0)
        {
            if (environment.UserId == 0)
            {
                return false;
            }
            return
                db.Collects.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.UserId == environment.UserId).Any();
        }

        public CollectLogEntity Save(CollectLogEntity data)
        {
            data.UserId = environment.UserId;
            db.Collects.Add(data);
            db.SaveChanges();
            return data;
        }

        public CollectLogEntity Add(int itemId, byte itemType = 0, string extraData = "")
        {
            return Save(new CollectLogEntity()
            {
                ItemId = itemId,
                ItemType = itemType,
                ExtraData = extraData
            });
        }


        /**
         * 获取评分的统计信息
         * @param int itemId
         * @param int itemType
         * @return int
         */
        public int Count(int itemId, byte itemType = 0)
        {
            return db.Collects.Where(i => i.ItemId == itemId && i.ItemType == itemType).Count();
        }

        public void RemoveBySelf(int id)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.Collects.Where(i => i.Id == id && i.UserId == environment.UserId).ExecuteDelete();
        }

        public void RemoveByItem(int itemId, byte itemType = 0)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.Collects.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.UserId == environment.UserId).ExecuteDelete();
        }
    }
}
