using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Repositories
{
    public class FriendRepository(
        IDatabase db, 
        IClientEnvironment environment,
        IUserRepository userStore)
    {

        public Page<FriendModel> GetList(
            string keywords = "", 
            int group = -1, int page = 1) {
            var sql = new Sql();
            sql.Select().From<FriendEntity>(db)
                .Where("belong_id=@0", environment.UserId);
            SearchHelper.Where(sql, "name", keywords);
            if (group >= 0)
            {
                sql.Where("classify_id=@0", group);
            }
            var items = db.Page<FriendModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public FriendGroupModel[] All() {
            var groupItems = db.Fetch<FriendClassifyEntity>("WHERE user_id=@0", environment.UserId);
            var data = new Dictionary<int, FriendGroupModel>()
            {
                {1, new FriendGroupModel(1, "默认分组") }
            };
            foreach (var item in groupItems) {
                data.Add(item.Id, new(item));
            }
            data.Add(0, new(0, "黑名单"));
            var items = db.Fetch<FriendModel>("WHERE belong_id=@0", environment.UserId);
            userStore.WithUser(items);
            foreach (var item in items) {
                if (!data.TryGetValue(item.ClassifyId, out var group))
                {
                    group = data[1];
                }
                group.Users.Add(item);
            }
            return data.Values.ToArray();
        }

        public Page<IUser> Search(string keywords = "", int page = 1) {
            var sql = new Sql();
            sql.Select("user_id").From<FriendEntity>(db)
                .Where("belong_id=@0", environment.UserId);
            var exclude = db.Pluck<int>(sql).ToList();
            exclude.Add(environment.UserId);
            return userStore.Search(keywords, page, [..exclude], true);
        }


        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="remark"></param>
        /// <exception cref=""></exception>
        public void Follow(int user, int group, string remark = "") {
            var model = db.First<FriendEntity>("WHERE user_id=@0 AND belong_id=@1", user, environment.UserId);
            if (model is null) {
                return;
            }
            if (!HasClassify(group)) {
                throw new Exception("选择的分组错误");
            }
            var userModel = userStore.Get(user);
            if (userModel is null) {
                throw new Exception("用户不存在");
            }
            var count = db.FindCount<int, FriendEntity>("belong_id=@0 AND user_id=@1", user, environment.UserId);
            db.Insert(new FriendEntity()
            {
                Name = userModel.Name,
                ClassifyId = group,
                UserId = userModel.Id,
                BelongId = environment.UserId,
                Status = group > 0 && count > 0 ? 1 : 0
            });
            var logCount = db.FindCount<int, ApplyEntity>("item_id=@0 and item_type=0 and user_id=@1", environment.UserId, user);
            if (logCount > 0) {
                var sql = new Sql();
                sql.Where("user_id=@0 AND item_type=0 AND item_id=@1 AND status=0", 
                    user, environment.UserId);
                db.Update<ApplyEntity>(sql, new Dictionary<string, object>()
                {
                    {"status", 1},
                    { MigrationTable.COLUMN_UPDATED_AT, environment.Now }
                });
            }
            if (count > 0) {
                var sql = new Sql();
                sql.Where("belong_id=@0 AND user_id=@1",
                    user, environment.UserId);
                db.Update<FriendEntity>(sql, new Dictionary<string, object>()
                {
                    {"status", group > 0 ? 1 : 0},
                });
                return;
            }
            if (logCount > 0) {
                return;
            }
            db.Insert(new ApplyEntity()
            {
                ItemType = 0,
                ItemId = userModel.Id,
                Remark = remark,
                UserId = environment.UserId,
                Status = 0
            });
        }

        /**
         * 取消关注
         * @param int user
         * @throws \Exception
         */
        public void Remove(int user) {
            db.DeleteWhere<FriendEntity>("user_id=@0 and belong_id=@1", user, environment.UserId);
            var sql = new Sql();
            sql.Where("belong_id=@0 AND user_id=@1",
                user, environment.UserId);
            db.Update<FriendEntity>(sql, new Dictionary<string, object>()
                {
                    {"status", 0},
                });
        }

        public void Move(int user,int group) {
            if (group >= 10) {
                var exist = db.FindCount<FriendClassifyEntity>("user_id=@0 and id=@1", environment.UserId, group) > 0;
                if (!exist) {
                    throw new Exception("分组错误");
                }
            }
            var count = db.FindCount<FriendEntity>("belong_id=@0 and user_id=@1",
                environment.UserId, user);
            if (count < 0) {
                throw new Exception("好友错误");
            }
            var followed = db.FindCount<FriendEntity>("belong_id=@0 AND user_id=@1",
                user, environment.UserId);
            var sql = new Sql();
            sql.Where("belong_id=@0 AND user_id=@1",
                environment.UserId, user);
            db.Update<FriendEntity>(sql, new Dictionary<string, object>()
            {
                {"classify_id", group},
                {"status",  followed > 0 && group > 0 ? 1 : 0}
            });
            if (followed > 0) {
                sql = new Sql();
                sql.Where("belong_id=@0 AND user_id=@1",
                user, environment.UserId);
                db.Update<FriendEntity>(sql, new Dictionary<string, object>()
                {
                    {"status",  group < 1 ? 0 : 1}
                });
            }
        }

        public FriendEntity? Get(int user) {
            return db.First<FriendEntity>("WHERE belong_id=@0 and user_id=@1", environment.UserId, user);
        }

        /**
         * 所有分组
         * @return mixed
         * @throws \Exception
         */
        public List<FriendClassifyEntity> ClassifyList() {
            var items = db.Fetch<FriendClassifyEntity>("WHERE user_id=@0", 
                environment.UserId);
            items.Insert(0, new()
            {
                Id = 1,
                Name = "默认分组"
            });
            items.Add(new()
            {
                Id = 0,
                Name = "黑名单"
            });
            return items;
        }

        public FriendClassifyEntity ClassifySave(string name, int id = 0) {
            var model = id > 0 ? db.First<FriendClassifyEntity>("WHERE id=@0 AND user_id=@1",
                id, environment.UserId) : new FriendClassifyEntity();
            model.Name = name;
            model.UserId = environment.UserId;
            if (!db.TrySave(model)) {
                throw new Exception("error");
            }
            return model;
        }

        public void ClassifyRemove(int id) {
            if (id < 10) {
                throw new Exception("系统分组无法删除");
            }
            var model = db.First<FriendClassifyEntity>("WHERE user_id=@0 and id=@1",
                environment.UserId, id);
            if (model is null) {
                throw new Exception("无法删除");
            }
            db.DeleteById<FriendClassifyEntity>(model.Id);
            var sql = new Sql();
            sql.Where("belong_id=@0 AND classify_id=@1",
                environment.UserId, id);
            db.Update<FriendEntity>(sql, new Dictionary<string, object>()
                {
                    {"classify_id", 1},
                });
        }

        public bool HasClassify(int id) {
            if (id < 10) {
                return true;
            }
            return db.FindCount<FriendClassifyEntity>("user_id=@0 and id=@1", environment.UserId, id) > 0;
        }

        /**
         * 我关注的
         * @return int
         * @throws \Exception
         */
        public int FollowCount() {
            return db.FindCount<FriendEntity>("belong_id=@0 and classify_id>0", environment.UserId);
        }

        /**
         * 关注我的
         * @return int
         * @throws \Exception
         */
        public int FollowedCount() {
            return db.FindCount<FriendEntity>("user_id=@0 and classify_id>0", environment.UserId);
        }

        public Page<ApplyModel> ApplyLog(int page = 1) {
            var sql = new Sql();
            sql.Select().From<ApplyEntity>(db)
                .Where("item_type=0 AND item_id=@0", environment.UserId)
                .OrderBy("status asc, id desc");
            var items = db.Page<ApplyModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }
        
    }
}
