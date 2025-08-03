using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class GroupRepository(ChatContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public GroupListItem[] All()
        {
            var ids = db.GroupUsers.Where(i => i.UserId == client.UserId)
                .Select(i => i.GroupId)
                .ToArray();
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Groups.Where(i => ids.Contains(i.Id))
                .SelectAs()
                .ToArray();
        }

        public IOperationResult<GroupModel> Detail(int id)
        {
            if (!Canable(id))
            {
                return OperationResult<GroupModel>.Fail("无权限查看");
            }
            var model = db.Groups.Where(i => i.Id == id).Single()?.CopyTo<GroupModel>();
            if (model == null)
            {
                return OperationResult<GroupModel>.Fail("群不存在");
            }
            model.Users = Users(id);
            return OperationResult.Ok(model);
        }

        public IPage<GroupUserModel> Users(int id, string keywords = "", int page = 1)
        {
            var items = db.GroupUsers.Search(keywords, "name")
                .Where(i => i.GroupId == id).ToPage(page)
                .CopyTo<GroupUserEntity, GroupUserModel>();
            userStore.Include(items.Items);
            return items;
        }

        public IPage<GroupListItem> Search(QueryForm form)
        {
            var ids = db.GroupUsers.Where(i => i.UserId == client.UserId)
                .Select(i => i.GroupId)
                .ToArray();
            return db.Groups.When(ids.Length > 0, i => !ids.Contains(i.Id))
                .ToPage(form, i => i.SelectAs());
        }

        public bool Canable(int id)
        {
            return db.GroupUsers.Where(i => i.GroupId == id && i.UserId == client.UserId).Any();
        }

        public bool Manageable(int id)
        {
            return db.Groups.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }

        public IOperationResult Agree(ApplyForm data)
        {
            if (data.Group < 1)
            {
                return OperationResult.Fail("错误");
            }
            if (!Manageable(data.Group)) {
                return OperationResult.Fail("无权限处理");
            }
            var exist = db.GroupUsers.Where(i => i.GroupId == data.Group && i.UserId == data.User).Any();
            if (exist)
            {
                return OperationResult.Fail("已处理过");
            }
            var userModel = userStore.Get(data.User);
            if (userModel is null)
            {
                db.Applies.Where(i => i.UserId == data.User && i.ItemType == 1 && i.ItemId == data.Group)
                    .ExecuteDelete();
                db.SaveChanges();
                return OperationResult.Fail("用户不存在");
            }
            db.GroupUsers.Save(new GroupUserEntity()
            {
                GroupId = data.Group,
                UserId = data.User,
                Name = userModel.Name,
                RoleId = 0,
                Status = 5,
            }, client.Now);
            db.Applies.Where(i => i.UserId == data.User && i.ItemType == 1 && i.ItemId == data.Group && i.Status == 0)
                  .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1)
                  .SetProperty(i => i.UpdatedAt, client.Now));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Apply(ApplyForm data)
        {
            if (data.Group < 1)
            {
                return OperationResult.Fail("错误");
            }
            if (Canable(data.Group)) 
            {
                return OperationResult.Fail("你已加入该群");
            }
            var exist = db.Groups.Where(i => i.Id == data.Group).Any();
            if (!exist)
            {
                return OperationResult.Fail("群不存在");
            }
            db.Applies.Save(new ApplyEntity()
            {
                ItemType = 1,
                ItemId = data.Group,
                Remark = data.Remark,
                UserId = client.UserId,
                Status = 0
            }, client.Now);
            return OperationResult.Ok();
        }

        public IPage<ApplyListItem> ApplyLog(int id, QueryForm form)
        {
            if (!Manageable(id)) {
                // throw new Exception("无权限处理");
                return new Page<ApplyListItem>();
            }
            var items = db.Applies.Where(i => i.ItemType == 1 && i.ItemId == id)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        /**
         * 创建群
         * @param array data
         */
        public IOperationResult<GroupEntity> Create(GroupForm data)
        {
            var model = new GroupEntity()
            {
                Name = data.Name,
                Description = data.Description,
                Logo = data.Logo,
                UserId = client.UserId
            };
            db.Groups.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<GroupEntity>.Fail("error");
            }
            db.GroupUsers.Save(new GroupUserEntity()
            {
                UserId = model.UserId,
                Name = userStore.Get(model.UserId)!.Name,
                GroupId = model.Id,
                RoleId = 99,
                Status = 5,
            }, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="id"></param>
        public IOperationResult Disband(int id)
        {
            var model = db.Groups.Where(i => i.Id == id).Single();
            if (model.UserId != client.UserId)
            {
                return OperationResult.Fail("无权限操作");
            }
            db.Groups.Where(i => i.Id == id).ExecuteDelete();
            db.GroupUsers.Where(i => i.GroupId == id).ExecuteDelete();
            db.Messages.Where(i => i.GroupId == id).ExecuteDelete();
            db.Histories.Where(i => i.ItemId == id && i.ItemType == 1).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

    }
}
