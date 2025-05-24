using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Data;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class ShareRepository(PlanContext db, 
        IClientContext client, IUserRepository userStore)
    {
        public IPage<ShareListItem> GetList(QueryForm form)
        {
            var ids = db.ShareUsers.Where(i => i.UserId == client.UserId 
                && i.DeletedAt == 0).Select(i => i.ShareId).ToArray();
            var res = db.Shares.Where(i => ids.Contains(i.Id))
                .OrderByDescending(i => i.Id)
                .ToPage<ShareEntity, ShareListItem>(form);
            userStore.Include(res.Items);
            TaskRepository.Include(db, res.Items);
            return res;
        }

        public IPage<ShareListItem> MyList(QueryForm form)
        {
            var res = db.Shares.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .ToPage<ShareEntity, ShareListItem>(form);
            TaskRepository.Include(db, res.Items);
            return res;
        }

        public bool IsShareUser(int[] taskIds, int userId)
        {
            var shareIds = db.Shares.Where(i => taskIds.Contains(i.TaskId))
                .Select(i => i.Id).ToArray();
            if (shareIds.Length == 0)
            {
                return false;
            }
            return db.ShareUsers.Where(i => shareIds.Contains(i.ShareId)
                && i.UserId == userId && i.DeletedAt == 0).Any();
        }

        public IOperationResult<ShareEntity> Create(ShareForm data)
        {
            var task = db.Tasks.Where(i => i.Id == data.TaskId && i.UserId == client.UserId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult<ShareEntity>.Fail("任务错误");
            }
            var share = new ShareEntity()
            {
                UserId = client.UserId,
                TaskId = data.TaskId,
                ShareRule = data.ShareRule,
                ShareType = data.ShareType,
            };
            db.Shares.Save(share, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(share);
        }

        public IOperationResult<ShareModel> Detail(int id)
        {
            var share = db.Shares.Where(i => i.Id == id).SingleOrDefault();
            if (share is null)
            {
                return OperationResult<ShareModel>.Fail("数据错误");
            }
            if (share.UserId != client.UserId)
            {
                AddUser(share, client.UserId);
            }
            var res = share.CopyTo<ShareModel>();
            res.Task = db.Tasks.Where(i => i.Id == share.TaskId).SingleOrDefault();
            return OperationResult.Ok(res);
        }

        public IOperationResult<UserRoleItem[]> Users(int id)
        {
            var share = db.Shares.Where(i => i.Id == id).SingleOrDefault();
            if (share is null)
            {
                return OperationResult.Fail<UserRoleItem[]>("数据错误");
            }
            var userIds = db.ShareUsers
                .Where(i => i.ShareId == id && i.DeletedAt == 0)
                .Select(i => i.UserId).ToList();
            userIds.Add(share.UserId);
            if (!userIds.Contains(client.UserId))
            {
                return OperationResult.Fail<UserRoleItem[]>("无权限操作");
            }
            var items = userStore.Get(userIds.ToArray());
            var res = items.Select(i => new UserRoleItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar,
                IsOnline = i.IsOnline,
                RoleName = i.Id == share.UserId ? "所有者" : string.Empty,
                Editable = share.UserId == client.UserId
            }).OrderByDescending(i => i.RoleName).ToArray();
            return OperationResult.Ok(res);
        }

        public IOperationResult AddUser(ShareEntity share, int user_id)
        {
            var log = db.ShareUsers.Where(i => i.ShareId == share.Id && i.UserId == user_id)
                .FirstOrDefault();
            if (log is not null && log.DeletedAt > 0)
            {
                return OperationResult.Fail("你已被移除");
            }
            if (log is not null)
            {
                return OperationResult.Ok();
            }
            if (client.Now - share.CreatedAt > 86400)
            {
                return OperationResult.Fail("分享已过期");
            }
            db.ShareUsers.Save(new ShareUserEntity()
            {
                ShareId = share.Id,
                UserId = user_id,
            }, client.Now);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Remove(int id)
        {
            var share = db.Shares.Where(i => i.Id == id).SingleOrDefault();
            if (share is null)
            {
                return OperationResult.Fail("分享错误");
            }
            if (share.UserId == client.UserId)
            {
                db.Shares.Remove(share);
                db.ShareUsers.Where(i => i.ShareId == share.Id).ExecuteDelete();
                db.SaveChanges();
                return OperationResult.Ok();
            }
            db.ShareUsers.Where(i => i.ShareId == share.Id && i.UserId == client.UserId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult RemoveUser(int id, int user_id)
        {
            var share = db.Shares.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (share is null)
            {
                return OperationResult.Fail("无权限操作");
            }
            db.ShareUsers.Where(i => i.ShareId == share.Id && i.UserId == user_id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now));
            db.SaveChanges();
            return OperationResult.Ok();
        }
        internal static bool IsShareUser(PlanContext db, int[] taskItems, int user)
        {
            return new ShareRepository(db, null, null).IsShareUser(taskItems, user);
        }
    }
}