using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;

namespace NetDream.Shared.Providers
{
    public class CollectProvider(IDatabase db, string prefix, IClientEnvironment environment) : IMigrationProvider
    {
        private readonly string _tableName = prefix + "_collect";
        public void Migration(IMigration migration)
        {
            migration.Append(_tableName, table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.String("extra_data").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }

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
            db.Delete(_tableName, id);
        }

        public int Insert(CollectLog data)
        {
            var id = db.Insert(_tableName, data);
            if (id == 0)
            {
                throw new Exception("insert log error");
            }
            return id;
        }

        public void Update(int id, CollectLog data)
        {
            data.Id = id;
            db.TryUpdate(_tableName, data);
        }

        public CollectLog? Get(int id)
        {
            return db.FindById<CollectLog>(_tableName, id);
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
            return db.FindCount(_tableName, "item_id=@0 AND item_type=@1 AND user_id=@2",
                itemId, itemType, environment.UserId) > 0;
        }

        public CollectLog Save(CollectLog data)
        {
            data.UserId = environment.UserId;
            data.Id = db.Insert(_tableName, data);
            // data["user"] = UserSimpleModel.ConverterFrom(auth().User());
            return data;
        }

        public CollectLog Add(int itemId, byte itemType = 0, string extraData = "")
        {
            return Save(new CollectLog()
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
            return db.FindCount(_tableName, "item_id=@0 AND item_type=@1",
                itemId, itemType);
        }

        public void RemoveBySelf(int id)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.Delete(_tableName, "id=@0 AND user_id=@1", id, environment.UserId);
        }

        public void RemoveByItem(int itemId, byte itemType = 0)
        {
            if (environment.UserId == 0)
            {
                return;
            }
            db.DeleteWhere(_tableName, "item_id=@0 AND item_type=@1 AND user_id=@2", itemId, itemType, environment.UserId);
        }
    }
}
