using NetDream.Shared.Helpers;
using NetDream.Modules.UserAccount.Entities;
using System;
using NetDream.Shared.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers;
using NetDream.Shared.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class FundAccount(UserContext db): IFundAccount
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

        public const byte STATUS_WAITING_PAY = 0;
        public const byte STATUS_PAID = 1;
        public const byte STATUS_REFUND = 9;

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
            var now = TimeHelper.TimestampNow();
            return Change(new AccountLogEntity()
            {
                UserId = userId,
                Type = type,
                ItemId = itemId,
                Money = (int)money,
                TotalMoney = (int)totalMoney,
                Remark = remark,
                Status = status,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        public float GetUserMoney(int userId)
        {
            return db.Users.Where(i => i.Id == userId).Value(i => i.Money);
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

        public IFundOperation<T> Trading<T>(T data) where T : IFundRequest
        {
            return new FundOperation<T>(db, data);
        }

        public bool IsBought(int sourceId, FundOperateType sourceType)
        {
            return IsBought(sourceId, (byte)sourceType);
        }

        public bool IsBought(int sourceId, byte sourceType)
        {
            return db.AccountLogs.Where(i => i.ItemId == sourceId && i.Type == sourceType 
            && i.Status != STATUS_REFUND).Any();
        }
    }

    public class FundOperation<T>(UserContext db, T data) : IFundOperation<T>
        where T : IFundRequest
    {
        private IDbContextTransaction? _transaction;
        private readonly int _timestamp = TimeHelper.TimestampNow();
        private AccountLogEntity? _log;
        public T Source => data;

        public IOperationResult Commit()
        {
            if (data.Payer == data.Payee)
            {
                return OperationResult.Fail("不能自己付款给自己");
            }
            if (data.Value <= 0)
            {
                return OperationResult.Fail("金额不能为负");
            }
            if (_log is not null)
            {
                return OperationResult.Ok(_log.Id);
            }
            var money = (int)data.Value;
            var oldData = db.Users.Where(i => i.Id == data.Payer || i.Id == data.Payee)
                .Select(i => new KeyValuePair<int, int>(i.Id, i.Money)).ToDictionary();
            if (data.Payer > 0 && (!oldData.TryGetValue(data.Payer, out var m) || m < money))
            {
                return OperationResult.Fail("支付方余额不足");
            }
            if (data.Payee > 0 && !oldData.ContainsKey(data.Payee))
            {
                return OperationResult.Fail("收款方不存在");
            }
            _transaction = db.Database.BeginTransaction();
            if (data.Payer > 0)
            {
                _log = new AccountLogEntity()
                {
                    UserId = data.Payer,
                    Type = (byte)data.SourceType,
                    ItemId = data.SourceId,
                    Money = - money,
                    TotalMoney = oldData[data.Payer] - money,
                    Remark = data.Remark,
                    Status = FundAccount.STATUS_PAID,
                    CreatedAt = _timestamp,
                    UpdatedAt = _timestamp
                };
                db.AccountLogs.Add(_log);
                db.Users.Where(i => i.Id == data.Payer)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Money, _log.TotalMoney));
            }
            if (data.Payee > 0)
            {
                var log = new AccountLogEntity()
                {
                    UserId = data.Payee,
                    Type = (byte)data.SourceType,
                    ItemId = data.SourceId,
                    Money = money,
                    TotalMoney = oldData[data.Payee] + money,
                    Remark = data.Remark,
                    Status = FundAccount.STATUS_PAID,
                    CreatedAt = _timestamp,
                    UpdatedAt = _timestamp
                };
                db.AccountLogs.Add(log);
                db.Users.Where(i => i.Id == data.Payee)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Money, log.TotalMoney));
                _log ??= log;
            }
            db.SaveChanges();
            _transaction.Commit();
            return OperationResult.Ok(_log!.Id);
        }

        public IOperationResult Rollback()
        {
            if (_log is null)
            {
                return OperationResult.Ok();
            }
            _transaction?.Rollback();
            _log = null;
            return OperationResult.Ok();
        }
    }
}
