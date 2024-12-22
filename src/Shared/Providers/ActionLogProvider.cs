using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class ActionLogProvider(
        ILogContext db, 
        IClientContext environment)
    {
        /// <summary>
        /// 切换记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns>{0: 取消，1: 更新为，2：新增}</returns>
        public byte ToggleLog(byte type, byte action, int id)
        {
            return ToggleLog(type, action, id, [action]);
        }

        /// <summary>
        /// 切换记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <param name="searchAction"></param>
        /// <returns></returns>
        public byte ToggleLog(byte type, byte action, int id, IList<byte> searchAction)
        {
            if (environment.UserId == 0)
            {
                return 0;
            }
            var log = db.Logs.Where(i => i.ItemId == id && i.ItemType == type && i.UserId == environment.UserId && searchAction.Contains(i.Action))
                .Single();
            if (log == null)
            {
                log = new LogEntity
                {
                    UserId = environment.UserId,
                    ItemId = id,
                    ItemType = type,
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


        public byte? GetAction(byte type, int id, IList<byte>? onlyAction = null)
        {
            if (environment.UserId == 0)
            {
                return null;
            }
            var query = db.Logs.Where(i => i.ItemId == id && i.ItemType == type && i.UserId == environment.UserId);
            if (onlyAction is not null)
            {
                query = query.Where(i => onlyAction.Contains(i.Action));
            }
            return query.Select(i => i.Action).Single();
        }

        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Count(byte type, byte action, int id)
        {
            return db.Logs.Where(i => i.ItemType == type && i.ItemId == id && i.Action == action).Count();
        }

        public bool Has(byte type, int id, byte action = 0)
        {
            if (environment.UserId == 0)
            {
                return false;
            }
            return db.Logs.Where(i => i.ItemType == type && i.ItemId == id && i.Action == action).Any();
        }

        public void Insert(LogEntity data)
        {
            if (string.IsNullOrWhiteSpace(data.Ip))
            {
                data.Ip = environment.Ip;
            }
            if (data.CreatedAt == 0)
            {
                data.CreatedAt = TimeHelper.TimestampNow();
            }
            if (data.UserId == 0)
            {
                data.UserId = environment.UserId;
            }
            db.Logs.Add(data);
            db.SaveChanges();
        }

        public void Update(int id, LogEntity data)
        {
            data.Id = id;
            db.Logs.Update(data);
        }

        public void Remove(int id)
        {
            db.Logs.Where(i => i.Id == id).ExecuteDelete();
        }

    }
}
