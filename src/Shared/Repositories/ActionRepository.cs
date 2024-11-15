using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Shared.Repositories
{
    public abstract class ActionRepository<T>(IDatabase db) where T : IActionEntity
    {

        protected string TableName => ModelHelper.TableName(typeof(T));
        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int ActionCount(int itemId, byte itemType, byte action)
        {
            return db.ExecuteScalar<int>($"SELECT COUNT(*) as count FROM {TableName} WHERE item_id=@0 AND item_type=@1 AND action=@2", itemId, itemType, action);
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
            var count = db.ExecuteScalar<int>($"SELECT COUNT(*) as count FROM {TableName} WHERE item_id=@0 AND item_type=@1 AND action=@2 AND user_id=@3", itemId, itemType, action, userId);
            return count > 0;
        }

        /// <summary>
        /// 仅执行action,不做取消操作
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
            var log = Activator.CreateInstance<T>();
            log.UserId = userId;
            log.ItemId = itemId;
            log.ItemType = itemType;
            log.Action = action;
            log.CreatedAt = TimeHelper.TimestampNow();
            db.Insert(log);
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
        public byte? UserActionValue(int userId, int itemId, byte itemType, IList<byte> onlyAction)
        {
            return db.ExecuteScalar<byte>($"SELECT action FROM {TableName} WHERE item_id=@0 AND item_type=@1 AND user_id=@2 AND action IN ({string.Join(',', onlyAction)})", itemId, itemType, userId);
        }

        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public byte? UserActionValue(int userId, int itemId, byte itemType)
        {
            return db.ExecuteScalar<byte>($"SELECT action FROM {TableName} WHERE item_id=@0 AND item_type=@1 AND user_id=@2", itemId, itemType, userId);
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
            var log = db.First<T>($"WHERE item_id=@0 AND item_type=@1 AND user_id=@2 AND action IN ({string.Join(',', searchAction)})", itemId, itemType, userId);
            if (log == null)
            {
                log = Activator.CreateInstance<T>();
                log.UserId = userId;
                log.ItemId = itemId;
                log.ItemType = itemType;
                log.Action = action;
                log.CreatedAt = TimeHelper.TimestampNow();
                db.Insert(log);
                return 2;
            }
            if (log.Action == action)
            {
                db.Delete(log);
                return 0;
            }
            log.Action = action;
            log.CreatedAt = TimeHelper.TimestampNow();
            db.Update(log);
            return 1;
        }
    }
}
