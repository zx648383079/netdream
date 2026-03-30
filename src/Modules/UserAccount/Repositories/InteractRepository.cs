using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class InteractRepository(UserContext db) : IInteractRepository
    {

        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int Count(ModuleTargetType type, int target, InteractType action)
        {
            return db.InteractLogs.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.Action == (byte)action).Count();
        }
        /// <summary>
        /// 当前用户是否执行操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Has(int user, ModuleTargetType type, int target, InteractType action)
        {
            if (user <= 0)
            {
                return false;
            }
            return db.InteractLogs.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.Action == (byte)action && i.UserId == user).Any();
        }

        /// <summary>
        /// 仅执行 action,不做取消操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Add(int user, ModuleTargetType type, int target, InteractType action)
        {
            if (user <= 0)
            {
                return false;
            }
            if (Has(user, type, target, action))
            {
                return true;
            }
            var log = new InteractLogEntity()
            {
                UserId = user,
                ItemId = target,
                ItemType = (byte)type,
                Action = (byte)action,
                CreatedAt = TimeHelper.TimestampNow()
            };
            db.InteractLogs.Add(log);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="onlyAction"></param>
        /// <returns></returns>
        public InteractType Get(int user, ModuleTargetType type, int target, InteractType[] onlyAction)
        {
            var maps = Array.ConvertAll(onlyAction, i => (byte)i);
            return (InteractType)db.InteractLogs.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.UserId == user && maps.Contains(i.Action)).Select(i => i.Action).SingleOrDefault();
        }

        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public InteractType Get(int user, ModuleTargetType type, int target)
        {
            return (InteractType)db.InteractLogs.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.UserId == user).Select(i => i.Action).SingleOrDefault();
        }
        /// <summary>
        /// 取消或执行某个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Toggle(int user, ModuleTargetType type, int target, InteractType action)
        {
            return Update(user, type, target, action) > 0;
        }


        public RecordToggleType Update(int user, ModuleTargetType type, int target, InteractType action)
        {
            return Update(user, type, target, action, [action]);
        }

        public RecordToggleType Update(int user, ModuleTargetType type, int target, InteractType action, InteractType[] searchAction)
        {
            if (user <= 0)
            {
                return RecordToggleType.Deleted;
            }
            var maps = Array.ConvertAll(searchAction, i => (byte)i);
            var log = db.InteractLogs.Where(i => i.ItemId == target && i.ItemType == (byte)type && i.UserId == user && maps.Contains(i.Action)).SingleOrDefault();
            if (log == null)
            {
                log = new InteractLogEntity
                {
                    UserId = user,
                    ItemId = target,
                    ItemType = (byte)type,
                    Action = (byte)action,
                    CreatedAt = TimeHelper.TimestampNow()
                };
                db.InteractLogs.Add(log);
                db.SaveChanges();
                return RecordToggleType.Added;
            }
            if (log.Action == (byte)action)
            {
                db.InteractLogs.Remove(log);
                db.SaveChanges();
                return RecordToggleType.Deleted;
            }
            log.Action = (byte)action;
            log.CreatedAt = TimeHelper.TimestampNow();
            db.SaveChanges();
            return RecordToggleType.Updated;
        }
    }
}
