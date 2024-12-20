using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Auth.Models;
using NPoco;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;
using NetDream.Modules.Auth.Events;
using NetDream.Shared.Migrations;
using System.Data;
using System;
using NetDream.Shared.Repositories;
using NetDream.Modules.Auth.Forms;
using MediatR;

namespace NetDream.Modules.Auth.Repositories
{
    public class UserRepository(IDatabase db, IClientContext client, IMediator mediator) 
        : MetaRepository<UserMetaEntity>(db)
    {
        public const int STATUS_DELETED = 0; // 已删除
        public const int STATUS_FROZEN = 2; // 账户已冻结
        public const int STATUS_UN_CONFIRM = 9; // 邮箱注册，未确认邮箱
        public const int STATUS_ACTIVE = 10; // 账户正常
        public const int STATUS_ACTIVE_VERIFIED = 15; // 账户正常&实名认证了

        public const int SEX_MALE = 1; // 性别男
        public const int SEX_FEMALE = 2; //性别女

        protected override string IdKey => "user_id";

        protected override Dictionary<string, object> DefaultItems => new()
        {
            {"address_id", 0 }, // 默认收货地址
            { "id_card", "" } // 身份证
        };


        public bool IsVerified(UserEntity entity)
        {
            return IsVerified(entity.Status);
        }

        public bool IsVerified(int status)
        {
            return status == STATUS_ACTIVE_VERIFIED;
        }

        public bool IsActive(UserEntity entity)
        {
            return IsActive(entity.Status);
        }

        public bool IsActive(int status)
        {
            return status >= STATUS_ACTIVE;
        }

        public UserProfile? GetCurrentProfile(string extra = "")
        {
            if (client.UserId <= 0)
            {
                return null;
            }
            var model = db.SingleById<UserEntity>(client.UserId);

            return new UserProfile()
            {

            };
        }

        public UserProfile? GetPublicProfile(int id, string extra = "")
        {
            var model = db.FindFirst<UserEntity>("id,name,avatar,mobile,email,sex,status,created_at", "id=@0", id);
            if (model is null)
            {
                return null;
            }
            return new UserProfile()
            {

            };
        }

        public string GetLastIp(int user)
        {
            var ip = db.FindScalar<string, LoginLogEntity>("ip", "user_id=@0 and status=1 order by created_at desc", user);
            if (ip is null)
            {
                return string.Empty;
            }
            return StrHelper.HideIp(ip);
        }

        public UserEquityCard[] GetCardItems(int user)
        {
            return new CardRepository(db, client).GetUserCard(user);
        }

        public int GetBulletinCount(int user)
        {
            return db.FindCount<BulletinUserEntity>("user_id=@0 and status=0", user);
        }

        public int GetPostCount(int user)
        {
            return 0; // BlogRepository.GetPostCount(user);
        }

        public int GetFollowingCount(int user)
        {
            return new RelationshipRepository(db, client).FollowingCount(user);
        }

        public int GetFollowerCount(int user)
        {
            return new RelationshipRepository(db, client).FollowerCount(user);
        }

        public bool GetTodayCheckIn(int user)
        {
            return false; //CheckinRepository.TodayIsChecked(user);
        }

        public Page<UserEntity> GetAll(string keywords = "", 
            string sort = "id", string order = "desc",
            int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
               "id", MigrationTable.COLUMN_CREATED_AT,
                "name", "email",
                "status", "sex", "money", "credits"
            ]);
            var sql = new Sql();
            sql.Select().From<UserEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            sql.OrderBy($"{sort} {order}");
            var items = db.Page<UserEntity>(page, 20, sql);
            return items;
        }

        public Page<UserSimpleModel> SearchUser(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<UserEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            sql.OrderBy("id desc");
            return db.Page<UserSimpleModel>(page, 20, sql);
        }


        public void SaveIDCard(int id, string idCard = "")
        {
            SaveBatch(id, new()
            {
                {"id_card", idCard }
            });
            db.UpdateWhere<UserEntity>("status=" + (string.IsNullOrWhiteSpace(idCard) ? STATUS_ACTIVE : STATUS_ACTIVE_VERIFIED),
                "id=@0 and status>=@1", id, STATUS_ACTIVE);
           
        }

        /**
         * 保存用户
         * @param array data
         * @param array roles
         * @return UserModel
         * @throws Exception
         */
        public UserEntity Save(UserForm data, int[] roles)
        {
            if (data.Password != data.ConfirmPassword)
            {
                throw new Exception("两次密码不一致！");
            }
            var model = data.Id > 0 ? db.SingleById<UserEntity>(data.Id) : new UserEntity();
            // TODO
            if (!string.IsNullOrWhiteSpace(data.Password))
            {
                // 
            }

            if (!db.TrySave(model))
            {
                throw new Exception("");
            }
            SaveRoles(model.Id, roles);
            mediator.Publish(ManageAction.Create(client, "user_edit", model.Name, Constants.TYPE_USER_UPDATE, model.Id));
            return model;
        }

        public void SaveRoles(int user, int[] roles)
        {
            var (add, _, remove) = ModelHelper.SplitId(roles,
                db.Pluck<int>(new Sql().Select("role_id").From<UserRoleEntity>(db).Where("user_id=@0", user))
            );
            if (remove.Count > 0)
            {
                db.DeleteWhere<UserRoleEntity>(
                    $"user_id={user} AND role_id IN ({string.Join(',', remove)})");
            }
            if (add.Count > 0)
            {
                db.InsertBatch<UserRoleEntity>(add.Select(i => new Dictionary<string, object>()
                {
                    {"role_id", i},
                    {"user_id", user}
                }));
            }
        }

        public void Remove(int id)
        {
            if (id == client.UserId)
            {
                throw new Exception("不能删除自己！");
            }
            var user = db.SingleById<UserEntity>(id);
            db.DeleteById<UserEntity>(id);
            mediator.Publish(new CancelAccount(user, client.Now));
            mediator.Publish(ManageAction.Create(client, "user_remove", user.Name, Constants.TYPE_USER_UPDATE, user.Id));
        }

        /**
         * 缓存用户的权限
         * @param int user
         * @return array [role => array, roles => string[], permissions => string[]]
         * @throws Exception
         */
        public UserRole RolePermission(int user)
        {
            return new RoleRepository(db, client, mediator).UserRolePermission(user);
        }

        public string GetName(int id)
        {
            return db.FindScalar<string, UserEntity>("name", "user_id=@0", id);
        }

    }
}
