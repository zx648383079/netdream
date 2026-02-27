using NetDream.Shared.Helpers;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Repositories
{
    public abstract class ActionRepository(ILogContext db)
    {

        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int ActionCount(int itemId, byte itemType, byte action)
        {
            return db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.Action == action).Count();
        }
        /// <summary>
        /// 当前用户是否执行操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool UserHasAction(int userId, int itemId, byte itemType, byte action)
        {
            if (userId <= 0)
            {
                return false;
            }
            return db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.Action == action && i.UserId == userId).Any(); ;
        }

        /// <summary>
        /// 仅执行 action,不做取消操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool UserOnlyAction(int userId, int itemId, 
            byte itemType, byte action)
        {
            if (userId <= 0)
            {
                return false;
            }
            if (UserHasAction(userId, itemId, itemType, action))
            {
                return true;
            }
            var log = new LogEntity()
            {
                UserId = userId,
                ItemId = itemId,
                ItemType = itemType,
                Action = action,
                CreatedAt = TimeHelper.TimestampNow()
            };
            db.Logs.Add(log);
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
        public byte UserActionValue(int userId, int itemId, byte itemType, IList<byte> onlyAction)
        {
            return db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.UserId == userId && onlyAction.Contains(i.Action)).Select(i => i.Action).SingleOrDefault();
        }

        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public byte UserActionValue(int userId, int itemId, byte itemType)
        {
            return db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.UserId == userId).Select(i => i.Action).SingleOrDefault();
        }
        /// <summary>
        /// 取消或执行某个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool ToggleAction(int userId, int itemId, byte itemType, byte action)
        {
            return ToggleLog(userId, itemId, itemType, action) > 0;
        }


        public byte ToggleLog(int userId, int itemId, byte itemType, byte action)
        {
            return ToggleLog(userId, itemId, itemType, action, [action]);
        }

        public byte ToggleLog(
            int userId, int itemId, byte itemType, byte action, IList<byte> searchAction)
        {
            if (userId <= 0)
            {
                return 0;
            }
            var log = db.Logs.Where(i => i.ItemId == itemId && i.ItemType == itemType && i.UserId == userId && searchAction.Contains(i.Action)).Single();
            if (log == null)
            {
                log = new LogEntity
                {
                    UserId = userId,
                    ItemId = itemId,
                    ItemType = itemType,
                    Action = action,
                    CreatedAt = TimeHelper.TimestampNow()
                };
                db.Logs.Add(log);
                db.SaveChanges();
                return 2;
            }
            if (log.Action == action)
            {
                db.Logs.Remove(log);
                db.SaveChanges();
                return 0;
            }
            log.Action = action;
            log.CreatedAt = TimeHelper.TimestampNow();
            db.SaveChanges();
            return 1;
        }
    }
}
