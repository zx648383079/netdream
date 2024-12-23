using NetDream.Shared.Helpers;
using NetDream.Modules.Auth.Entities;
using System;
using NetDream.Shared.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NetDream.Modules.Auth.Repositories
{
    public class FundAccount(AuthContext db): IFundAccount
    {
        public const byte TYPE_SYSTEM = 1; // 系统自动
        public const byte TYPE_RECHARGE = 6; // 用户充值
        public const byte TYPE_ADMIN = 9; // 管理员充值
        public const byte TYPE_AFFILIATE = 15; // 分销
        public const byte TYPE_BUY_BLOG = 21;
        public const byte TYPE_FORUM_BUY = 25;
        public const byte TYPE_DEFAULT = 99;
        public const byte TYPE_CHECK_IN = 30;
        public const byte TYPE_BANK = 31;
        public const byte TYPE_GAME = 40;
        public const byte TYPE_SHOPPING = 60;

        public AccountLogEntity Log(int userId, byte type, int itemId, float money, float totalMoney, 
            string remark, byte status = 0)
        {
            return Log(new AccountLogEntity()
            {
                UserId = userId,
                Type = type,
                ItemId = itemId,
                Money = (int)money,
                TotalMoney = (int)totalMoney,
                Remark = remark,
                Status = status
            });
        }

        public AccountLogEntity Log(AccountLogEntity entity)
        {
            if (entity.Id > 0)
            {
                db.AccountLogs.Update(entity);
                db.SaveChanges();
                return entity;
            }
            entity.UpdatedAt = entity.CreatedAt = TimeHelper.TimestampNow();
            db.AccountLogs.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public int Change(int userId, byte type, int itemId, float money, float totalMoney,
            string remark, byte status = 1)
        {
            return Change(new AccountLogEntity()
            {
                UserId = userId,
                Type = type,
                ItemId = itemId,
                Money = (int)money,
                TotalMoney = (int)totalMoney,
                Remark = remark,
                Status = status
            });
        }

        public float GetUserMoney(int userId)
        {
            return db.Users.Where(i => i.Id == userId).Select(i => i.Money).Single();
        }

        public int Change(AccountLogEntity entity)
        {
            if (entity.UserId < 0)
            {
                throw new ArgumentException("user Id error");
            }
            var oldMoney = GetUserMoney(entity.UserId);
            var newMoney = oldMoney + entity.Money;
            if (newMoney < 0) {
                return 0;
            }
            db.Users.Where(i => i.Id == entity.UserId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Money, newMoney));
            entity.TotalMoney = (int)newMoney;
            entity = Log(entity);
            return entity.Id;
        }
    }
}
