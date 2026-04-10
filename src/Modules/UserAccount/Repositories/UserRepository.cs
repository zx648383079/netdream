using NetDream.Shared.Interfaces;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using NetDream.Shared.Repositories;
using NetDream.Modules.UserAccount.Forms;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Models;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class UserRepository(UserContext db, 
        IClientContext client, 
        IEventBus mediator,
        IMetaRepository metaStore) 
    {

        public const int SEX_MALE = 1; // 性别男
        public const int SEX_FEMALE = 2; //性别女

        protected Dictionary<string, string> DefaultItems => new()
        {
            { "address_id", "0" }, // 默认收货地址
            { "id_card", "" } // 身份证
        };


        public static bool IsVerified(UserEntity entity)
        {
            return IsVerified(entity.Status);
        }

        public static bool IsVerified(byte status)
        {
            return status == (byte)AccountStatus.ActiveVerified;
        }

        public static bool IsActive(UserEntity entity)
        {
            return IsActive(entity.Status);
        }

        public static bool IsActive(byte status)
        {
            return status >= (byte)AccountStatus.Active;
        }

        public UserProfileModel? GetCurrentProfile(string extra = "")
        {
            if (client.UserId <= 0)
            {
                return null;
            }
            var model = db.Users.Where(i => i.Id == client.UserId).Single();
            var openItems = new UserOpenStatisticsRequest(model.Id, extra.Split(','));
            mediator.Publish(openItems).GetAwaiter().GetResult();
            return new UserProfileModel()
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = model.Avatar,
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
            var openItems = new UserOpenStatisticsRequest(model.Id, extra.Split(','));
            mediator.Publish(openItems).GetAwaiter().GetResult();
            return new UserProfile()
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = model.Avatar,
            };
        }

       


        public IPage<UserListItem> MangeList(QueryForm form)
        {
            SearchHelper.CheckSortOrder(form, [
               "id", "created_at",
                "name", "email",
                "status", "sex", "money", "credits"
            ]);
            return db.Users.Search(form.Keywords, "name")
                .OrderBy<UserEntity, int>(form.Sort, form.Order)
                .ToPage(form, i => i.SelectAs());
        }

        public IPage<UserLabelItem> SearchUser(QueryForm form)
        {
            return db.Users.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAsLabel());
        }


        public IOperationResult SaveIDCard(int id, string idCard = "")
        {
            metaStore.Update(ModuleTargetType.User, id, string.Empty, new Dictionary<string, string>()
            {
                {"id_card", idCard }
            });
            var status = string.IsNullOrWhiteSpace(idCard) ? AccountStatus.Active : AccountStatus.ActiveVerified;
            db.Users.Where(i => i.Id == id && i.Status >= (byte)AccountStatus.Active)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, (byte)status));
            return OperationResult.Ok();
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="data"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IOperationResult<UserEntity> Save(UserForm data)
        {
            if (data.Password != data.ConfirmPassword)
            {
                return OperationResult.Fail<UserEntity>("两次密码不一致！");
            }
            var model = data.Id > 0 ? db.Users.Where(i => i.Id == data.Id).SingleOrDefault() : new UserEntity();
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
            // mediator.Send(new UserRoleBinding(model.Id, data.Roles));
            mediator.Publish(ManageAction.Create(client, "user_edit", model.Name, 
                ModuleTargetType.UserUpdate, model.Id));
            return OperationResult.Ok(model);
        }
        
        

        public void Remove(int id)
        {
            if (id == client.UserId)
            {
                throw new Exception("不能删除自己！");
            }
            var user = db.Users.Where(i => i.Id == id).Single();
            db.Users.Remove(user);
            db.SaveChanges();
            mediator.Publish(new CancelAccount(user, client.Now));
            mediator.Publish(ManageAction.Create(client, "user_remove", user.Name,
                ModuleTargetType.UserUpdate, user.Id));
        }


        public string GetName(int id)
        {
            return db.Users.Where(i => i.Id == id).Value(i => i.Name) ?? string.Empty;
        }


        public StatisticsItem[] Statistics()
        {
            if (client.UserId <= 0)
            {
                return [];
            }
            var data = new UserStatisticsRequest(client.UserId);
            mediator.Publish(data).GetAwaiter().GetResult();
            return [..data.Result];
        }

        public IOperationResult<UserEditableModel> MangeGet(int id)
        {
            var model = db.Users.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<UserEditableModel>.Fail("数据错误");
            }
            var res = model.CopyTo<UserEditableModel>();
            return OperationResult.Ok(res);
        }

        public IOperationResult<UserProfileModel> ChangeAvatar(IUploadFile file)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<UserProfileModel> UpdateProfile(ProfileUpdateForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<UserProfileModel> UpdateAccount(AccountUpdateForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<EmailLabelItem> CheckEmail(string email)
        {
            var model = db.Users.Where(i => i.Email == email)
                .Select(i => new EmailLabelItem()
                {
                    Email = email,
                    Name = i.Name,
                    Avatar = i.Avatar,
                }).FirstOrDefault();
            return OperationResult.OkOrFail(model, "邮箱未注册");
        }

        public IDictionary<string, string> SettingGet()
        {
            return metaStore.Get(ModuleTargetType.User, client.UserId, string.Empty, new Dictionary<string, string>()
            {
                {"accept_new_bulletin", "1" },
                {"open_not_disturb", "0" },
                {"post_expiration", "0" },
            });
        }

        public IOperationResult SettingSave(IDictionary<string, string> data)
        {
            metaStore.Update(ModuleTargetType.User, client.UserId, string.Empty, data);
            return OperationResult.Ok();
        }
    }
}
