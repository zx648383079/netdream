using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class GroupRepository(IDatabase db, 
        IClientEnvironment environment,
        IUserRepository userStore)
    {
        public List<GroupEntity> All()
        {
            var sql = new Sql();
            sql.Select("group_id").From<GroupUserEntity>(db)
                .Where("user_id=@0", environment.UserId);
            var ids = db.Pluck<int>(sql, "group_id");
            if (!ids.Any())
            {
                return [];
            }
            sql = new Sql();
            sql.Select().From<GroupEntity>(db)
                .WhereIn("id", ids.ToArray());
            return db.Fetch<GroupEntity>(sql);
        }

        public GroupModel Detail(int id)
        {
            if (!Canable(id))
            {
                throw new Exception("无权限查看");
            }
            var model = db.SingleById<GroupModel>(id);
            model.Users = Users(id);
            return model;
        }

        public Page<GroupUserModel> Users(int id, string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<GroupUserModel>(db)
                .Where("group_id=@0", id);
            SearchHelper.Where(sql, "name", keywords);
            var items = db.Page<GroupUserModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public Page<GroupEntity> Search(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select("group_id").From<GroupUserEntity>(db)
                .Where("user_id=@0", environment.UserId);
            var ids = db.Pluck<int>(sql, "group_id");
            sql = new Sql();
            sql.Select().From<GroupEntity>(db);
            if (ids.Any())
            {
                sql.WhereNotIn("id", ids.ToArray());
            }
            return db.Page<GroupEntity>(page, 20, sql);
        }

        public bool Canable(int id)
        {
            return db.FindCount<int, GroupUserEntity>("group_id=@0 AND user_id=@1", id, environment.UserId) > 0;
        }

        public bool Manageable(int id)
        {
            return db.FindCount<int, GroupEntity>("id=@0 AND user_id=@1", id, environment.UserId) > 0;
        }

        public void Agree(int user, int id)
        {
            if (!Manageable(id)) {
                throw new Exception("无权限处理");
            }
            var exist = db.FindCount<int, GroupUserEntity>("group_id=@0 AND user_id=@1", id, user) > 0;
            if (exist)
            {
                throw new Exception("已处理过");
            }
            var userModel = userStore.Get(user);
            if (userModel is null)
            {
                db.DeleteWhere<ApplyEntity>(
                    "user_id=@0 AND item_type=1 AND item_id=@1", user, id);
                throw new Exception("用户不存在");
            }
            db.Insert(new GroupUserEntity()
            {
                GroupId = id,
                UserId = user,
                Name = userModel.Name,
                RoleId = 0,
                Status = 5,
            });
            var sql = new Sql();
            sql.Where("user_id=@0 AND item_type=1 AND item_id=@1", user, id);
            db.Update<ApplyEntity>(sql, new Dictionary<string, object>()
            {
                {"status", 1},
                { MigrationTable.COLUMN_UPDATED_AT, environment.Now }
            });
        }

        public void Apply(int id, string remark = "")
        {
            if (Canable(id)) {
                throw new Exception("你已加入该群");
            }
            var exist = db.FindCount<GroupEntity>("id=@0", id) > 0;
            if (!exist)
            {
                throw new Exception("群不存在");
            }
            db.Insert(new ApplyEntity()
            {
                ItemType = 1,
                ItemId = id,
                Remark = remark,
                UserId = environment.UserId,
                Status = 0
            });
        }

        public Page<ApplyModel> ApplyLog(int id, int page = 1)
        {
            if (!Manageable(id)) {
                throw new Exception("无权限处理");
            }
            var sql = new Sql();
            sql.Select().From<ApplyEntity>(db)
                .Where("item_type=1 AND item_id=@1", id)
                .OrderBy("status asc, id desc");
            var items = db.Page<ApplyModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        /**
         * 创建群
         * @param array data
         */
        public GroupEntity Create(GroupForm data)
        {
            var model = new GroupModel()
            {
                Name = data.Name,
                Description = data.Description,
                Logo = data.Logo,
                UserId = environment.UserId
            };
            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            db.Insert(new GroupUserEntity()
            {
                UserId = model.UserId,
                Name = userStore.Get(model.UserId)!.Name,
                GroupId = model.Id,
                RoleId = 99,
                Status = 5,
            });
            return model;
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="id"></param>
        public void Disband(int id)
        {
            var model = db.SingleById<GroupEntity>(id);
            if (model.UserId != environment.UserId)
            {
                throw new Exception("无权限操作");
            }
            db.DeleteById<GroupEntity>(id);
            db.DeleteWhere<GroupUserEntity>("group_id=@0", id);
            db.DeleteWhere<MessageEntity>("group_id=@0", id);
            db.DeleteWhere<HistoryEntity>("item_id=@0 AND item_type=1", id);
        }

    }
}
