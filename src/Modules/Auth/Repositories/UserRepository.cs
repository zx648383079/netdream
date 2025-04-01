using NetDream.Shared.Interfaces;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;
using NetDream.Modules.Auth.Events;
using System.Data;
using System;
using NetDream.Shared.Repositories;
using NetDream.Modules.Auth.Forms;
using MediatR;
using NetDream.Shared.Providers;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Models;

namespace NetDream.Modules.Auth.Repositories
{
    public class UserRepository(AuthContext db, IClientContext client, IMediator mediator) 
        : MetaRepository(db)
    {
        public const int STATUS_DELETED = 0; // 已删除
        public const int STATUS_FROZEN = 2; // 账户已冻结
        public const int STATUS_UN_CONFIRM = 9; // 邮箱注册，未确认邮箱
        public const int STATUS_ACTIVE = 10; // 账户正常
        public const int STATUS_ACTIVE_VERIFIED = 15; // 账户正常&实名认证了

        public const int SEX_MALE = 1; // 性别男
        public const int SEX_FEMALE = 2; //性别女

        protected override Dictionary<string, string> DefaultItems => new()
        {
            {"address_id", "0" }, // 默认收货地址
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
            var model = db.Users.Where(i => i.Id == client.UserId).Single();
            var openItems = new UserOpenStatistics(model.Id, extra.Split(','));
            mediator.Publish(openItems).GetAwaiter().GetResult();
            return new UserProfile()
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = model.Avatar,
                IsOnline = true,
                MetaItems = openItems.Result,
            };
        }

        public UserProfile? GetPublicProfile(int id, string extra = "")
        {
            var model = db.Users.Where(i => i.Id == client.UserId)
                .Select(i => new UserEntity { 
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                    Email = i.Email,
                    Mobile = i.Mobile,
                    Sex = i.Sex,
                    Status = i.Status,
                    CreatedAt = i.CreatedAt
                }).SingleOrDefault();
            if (model is null)
            {
                return null;
            }
            var openItems = new UserOpenStatistics(model.Id, extra.Split(','));
            mediator.Publish(openItems).GetAwaiter().GetResult();
            return new UserProfile()
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = model.Avatar,
                MetaItems = openItems.Result,
            };
        }

        public static string GetLastIp(AuthContext db, int user)
        {
            var ip = db.LoginLogs
                .Where(i => i.UserId == user && i.Status == 1)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.Ip)
                .Take(1)
                .SingleOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                return string.Empty;
            }
            return StrHelper.HideIp(ip);
        }


        public IPage<UserEntity> GetAll(string keywords = "", 
            string sort = "id", string order = "desc",
            int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
               "id", "created_at",
                "name", "email",
                "status", "sex", "money", "credits"
            ]);
            return db.Users.Search(keywords, "name")
                .OrderBy<UserEntity, object>(sort, order)
                .ToPage(page);
        }

        public IPage<UserListItem> SearchUser(string keywords = "", int page = 1)
        {
            return db.Users.Search(keywords, "name")
                .OrderByDescending(i => i.Id)
                .Select(i => new UserListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                }).ToPage(page);
        }


        public void SaveIDCard(int id, string idCard = "")
        {
            SaveBatch(id, new()
            {
                {"id_card", idCard }
            });
            var status = string.IsNullOrWhiteSpace(idCard) ? STATUS_ACTIVE : STATUS_ACTIVE_VERIFIED;
            db.Users.Where(i => i.Id == id && i.Status >= STATUS_ACTIVE)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, status));
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="data"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IOperationResult<UserEntity> Save(UserForm data, int[] roles)
        {
            if (data.Password != data.ConfirmPassword)
            {
                return OperationResult.Fail<UserEntity>("两次密码不一致！");
            }
            var model = data.Id > 0 ? db.Users.Where(i => i.Id == data.Id).Single() : new UserEntity();
            if (model is null)
            {
                return OperationResult.Fail<UserEntity>("user is error");
            }
            // TODO
            if (!string.IsNullOrWhiteSpace(data.Password))
            {
                // 
            }
            db.Users.Save(model);
            if (db.SaveChanges() == 0)
            {
                return OperationResult.Fail<UserEntity>("");
            }
            SaveRoles(model.Id, roles);
            mediator.Publish(ManageAction.Create(client, "user_edit", model.Name, ModuleModelType.TYPE_USER_UPDATE, model.Id));
            return OperationResult.Ok(model);
        }

        public void SaveRoles(int user, int[] roles)
        {
            var (add, _, remove) = ModelHelper.SplitId(roles,
                db.UserRoles.Where(i => i.UserId == user)
                .Select(i => i.RoleId).ToArray()
            );
            if (remove.Count > 0)
            {
                db.UserRoles.Where(i => i.UserId == user && remove.Contains(i.RoleId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.UserRoles.AddRange(add.Select(i => new UserRoleEntity()
                {
                    RoleId = i,
                    UserId = user,
                }));
                db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            if (id == client.UserId)
            {
                throw new Exception("不能删除自己！");
            }
            var user = db.Users.Where(i => i.Id == id).Single();
            db.Users.Remove(user);
            mediator.Publish(new CancelAccount(user, client.Now));
            mediator.Publish(ManageAction.Create(client, "user_remove", user.Name, ModuleModelType.TYPE_USER_UPDATE, user.Id));
        }

        /// <summary>
        /// 缓存用户的权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserRole RolePermission(int user)
        {
            return new RoleRepository(db, client, mediator).UserRolePermission(user);
        }

        public string GetName(int id)
        {
            return db.Users.Where(i => i.Id == id).Select(i => i.Name).Single();
        }

    }
}
