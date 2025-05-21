using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class FriendRepository(
        ChatContext db, 
        IClientContext client,
        IUserRepository userStore)
    {

        public IPage<FriendModel> GetList(
            string keywords = "", 
            int group = -1, int page = 1) {
            var items = db.Friends.Where(i => i.BelongId == client.UserId)
                .Search(keywords, "name").When(group >= 0, i =>i.ClassifyId == group)
                .ToPage(page).CopyTo<FriendEntity, FriendModel>();
            userStore.Include(items.Items);
            return items;
        }

        public FriendGroupModel[] All() {
            var groupItems = db.FriendClassifies.Where(i => i.UserId == client.UserId)
                .ToArray();
            var data = new Dictionary<int, FriendGroupModel>()
            {
                {1, new FriendGroupModel(1, "默认分组") }
            };
            foreach (var item in groupItems) {
                data.Add(item.Id, new(item));
            }
            data.Add(0, new(0, "黑名单"));
            var items = db.Friends.Where(i => i.BelongId == client.UserId)
                .ToArray().CopyTo<FriendEntity, FriendModel>();
            userStore.Include(items);
            foreach (var item in items) {
                if (!data.TryGetValue(item.ClassifyId, out var group))
                {
                    group = data[1];
                }
                group.Users.Add(item);
            }
            return data.Values.ToArray();
        }

        public IPage<IUser> Search(QueryForm form) {
            var exclude = db.Friends.Where(i => i.BelongId == client.UserId)
                .Select(i => i.UserId)
                .ToList();
            exclude.Add(client.UserId);
            return userStore.Search(form, [..exclude], true);
        }


        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="remark"></param>
        /// <exception cref=""></exception>
        public IOperationResult Follow(int user, int group, string remark = "") {
            var model = db.Friends.Where(i => i.UserId == user && i.BelongId == client.UserId)
                .Single();
            if (model is not null) {
                return OperationResult.Ok();
            }
            if (!HasClassify(group)) {
                return OperationResult.Fail("选择的分组错误");
            }
            var userModel = userStore.Get(user);
            if (userModel is null) {
                return OperationResult.Fail("用户不存在");
            }
            var count = db.
                Friends.Where(i => i.BelongId == user && i.UserId == client.UserId).Any();
            db.Friends.Save(new FriendEntity()
            {
                Name = userModel.Name,
                ClassifyId = group,
                UserId = userModel.Id,
                BelongId = client.UserId,
                Status = group > 0 && count ? 1 : 0
            }, client.Now);
            var logCount = db.Applies.Where(i => i.ItemId == client.UserId && i.ItemType == 0 && i.UserId == user).Any();
            if (logCount) {
                
                db.Applies.Where(i => i.UserId == user && i.ItemType == 0 && i.ItemId == client.UserId && i.Status == 0)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1)
                .SetProperty(i => i.UpdatedAt, client.Now));
            }
            if (count) {
                db.Friends.Where(i => i.BelongId == user && i.UserId == client.UserId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, group > 0 ? 1 : 0));
                return OperationResult.Ok();
            }
            if (logCount) {
                return OperationResult.Ok();
            }
            db.Applies.Save(new ApplyEntity()
            {
                ItemType = 0,
                ItemId = userModel.Id,
                Remark = remark,
                UserId = client.UserId,
                Status = 0
            }, client.Now);
            return OperationResult.Ok();
        }

        /**
         * 取消关注
         * @param int user
         * @throws \Exception
         */
        public void Remove(int user) 
        {
            db.Friends.Where(i => i.UserId == user && i.BelongId == client.UserId).ExecuteDelete();
            db.Friends.Where(i => i.BelongId == user && i.UserId == client.UserId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
        }

        public IOperationResult Move(int user,int group) 
        {
            if (group >= 10) {
                var exist = db.FriendClassifies.Where(i => i.UserId == client.UserId && i.Id == group).Any();
                if (!exist) {
                    return OperationResult.Fail("分组错误");
                }
            }
            var count = db.Friends.Where(i => i.BelongId == client.UserId && i.UserId == user).Any();
            if (!count) {
                return OperationResult.Fail("好友错误");
            }
            var followed = db.Friends.Where(i => i.BelongId == user && i.UserId == client.UserId).Any();
            db.Friends.Where(i => i.BelongId == client.UserId && i.UserId == user)
                .ExecuteUpdate(setters =>
                setters.SetProperty(i => i.ClassifyId, group)
                .SetProperty(i => i.Status, followed && group > 0 ? 1 : 0));
            if (followed) {
                db.Friends.Where(i => i.BelongId == user && i.UserId == client.UserId)
                   .ExecuteUpdate(setters =>
                   setters.SetProperty(i => i.Status, group < 1 ? 0 : 1));
            }
            return OperationResult.Ok();
        }

        public FriendEntity? Get(int user) 
        {
            return db.Friends.Where(i => i.BelongId == client.UserId && i.UserId == user).Single() ;
        }

        /**
         * 所有分组
         * @return mixed
         * @throws \Exception
         */
        public List<FriendClassifyEntity> ClassifyList() 
        {
            var items = db.FriendClassifies.Where(i => i.UserId == client.UserId).ToList();
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

        public IOperationResult<FriendClassifyEntity> ClassifySave(string name, int id = 0) 
        {
            var model = id > 0 ? db.FriendClassifies.Where(i => i.Id == id && i.UserId == client.UserId).Single() : new FriendClassifyEntity();
            if (model is null)
            {
                return OperationResult.Fail<FriendClassifyEntity>("group is error");
            }
            model.Name = name;
            model.UserId = client.UserId;
            db.FriendClassifies.Save(model, client.Now);
            if (db.SaveChanges() == 0) {
                return OperationResult.Fail<FriendClassifyEntity>("error");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult ClassifyRemove(int id) 
        {
            if (id < 10) {
                return OperationResult.Fail("系统分组无法删除");
            }
            var model = db.FriendClassifies.Where(i => i.UserId == client.UserId && i.Id == id).Single();
            if (model is null) {
                return OperationResult.Fail("无法删除");
            }
            db.FriendClassifies.Remove(model);
            db.SaveChanges();
            db.Friends.Where(i => i.BelongId == client.UserId && i.ClassifyId == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.ClassifyId, 1));
            return OperationResult.Ok();
        }

        public bool HasClassify(int id) {
            if (id < 10) {
                return true;
            }
            return db.FriendClassifies.Where(i => i.UserId == client.UserId && i.Id == id).Any();
        }

        /**
         * 我关注的
         * @return int
         * @throws \Exception
         */
        public int FollowCount() {
            return db.Friends.Where(i => i.BelongId == client.UserId && i.ClassifyId > 0).Count();
        }

        /**
         * 关注我的
         * @return int
         * @throws \Exception
         */
        public int FollowedCount() {
            return db.Friends.Where(i => i.UserId == client.UserId && i.ClassifyId > 0).Count();
        }

        public IPage<ApplyModel> ApplyLog(int page = 1) {
            var items = db.Applies.Where(i => i.ItemType == 0 && i.ItemId == client.UserId)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page).CopyTo<ApplyEntity, ApplyModel>();
            userStore.Include(items.Items);
            return items;
        }
        
    }
}
