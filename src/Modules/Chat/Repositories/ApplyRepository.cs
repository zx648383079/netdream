using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Chat.Repositories
{
    public class ApplyRepository(
        IDatabase db,
        IClientContext environment,
        IUserRepository userStore,
        GroupRepository groupStore
        )
    {
        public Page<ApplyModel> GroupList(int id = 0, int page = 1)
        {
            if (!groupStore.Manageable(id))
            {
                throw new Exception("无权限处理");
            }
            var sql = new Sql();
            sql.Select().From<ApplyEntity>(db)
                .Where("item_type=1 AND item_id=@0", id)
                .OrderBy("status asc, id desc");
            var items = db.Page<ApplyModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public Page<ApplyModel> GetList(int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<ApplyEntity>(db)
                .Where("item_type=0 AND item_id=@0", environment.UserId)
                .OrderBy("status asc, id desc");
            var items = db.Page<ApplyModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public void RemoveMy()
        {
            db.DeleteWhere<ApplyEntity>("item_type=0 AND item_id=@0", environment.UserId);
        }

        public void RemoveGroup(int id)
        {
            if (!groupStore.Manageable(id))
            {
                throw new Exception("无权限处理");
            }
            db.DeleteWhere<ApplyEntity>("item_type=1 AND item_id=@0", id);
        }

        public void Agree(int user)
        {
            var sql = new Sql();
            sql.Where("user_id=@0 AND item_type=0 AND item_id=@1 AND status=0", user, environment.UserId);
            db.Update<ApplyEntity>(sql, new Dictionary<string, object>()
            {
                {"status", 1},
                { MigrationTable.COLUMN_UPDATED_AT, environment.Now }
            });

        }

        public void AgreeGroup(int user, int id)
        {
            var sql = new Sql();
            sql.Where("user_id=@0 AND item_type=1 AND status=0 AND item_id=@1", user, id);
            db.Update<ApplyEntity>(sql, new Dictionary<string, object>()
            {
                {"status", 1},
                { MigrationTable.COLUMN_UPDATED_AT, environment.Now }
            });
        }

        public void Apply(int type, int id, string remark = "")
        {
            db.Insert(new ApplyEntity()
            {
                ItemType = type,
                ItemId = id,
                Remark = remark,
                UserId = environment.UserId,
                Status = 0
            });
        }
    }
}
