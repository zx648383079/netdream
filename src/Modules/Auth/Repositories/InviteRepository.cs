using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class InviteRepository(AuthContext db, IClientContext client)
    {
        public const int TYPE_CODE = 0; // 邀请码
        public const int TYPE_LOGIN = 5; // 扫码登录

        public const int STATUS_UN_SCAN = 0;  //未扫码
        public const int STATUS_UN_CONFIRM = 1;  // 已扫码待确认
        public const int STATUS_SUCCESS = 2;     // 登录成功
        public const int STATUS_REJECT = 3;      // 拒绝登录

        public IPage<InviteCodeEntity> CodeList(string keywords = "", int user = 0, int page = 1)
        {
            return db.InviteCodes.Where(i => i.Type == TYPE_CODE)
                .When(keywords, i => i.Token == keywords)
                .When(user > 0, i => i.UserId == user)
                .OrderByDescending(i => i.Id).ToPage(page);
        }
        public string CodeCreate(int amount = 1, string expiredAt = "")
        {
            return CodeCreate(amount, string.IsNullOrWhiteSpace(expiredAt) ? 0 : TimeHelper.TimestampFrom(expiredAt));
        }
        public string CodeCreate(int amount = 1, int expiredAt = 0)
        {
            return CreateNew(TYPE_CODE, amount, expiredAt);
        }

        private bool HasCode(string code)
        {
            return db.InviteCodes.Where(i => i.Token == code)
                .Where(i => i.ExpiredAt > client.Now || i.ExpiredAt == 0).Any();
        }

        /**
         * 判断邀请码是否有效
         * @param string code
         * @return InviteCodeModel|null
         */
        public InviteCodeEntity? FindCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }
            var model = db.InviteCodes.Where(i => i.Token == code)
                .Where(i => i.ExpiredAt > client.Now || i.ExpiredAt == 0).Single();
            if (model is null || (model.Amount > 0 && model.InviteCount == model.Amount))
            {
                return null;
            }
            return model;
        }

        public void CodeRemove(int id)
        {
            db.InviteCodes.Where(i => i.Id == id && i.Type == TYPE_CODE).ExecuteDelete();
        }

        public void CodeClear()
        {
            db.InviteCodes.Where(i => i.Type == TYPE_CODE).ExecuteDelete();
        }

        public IPage<InviteLogEntity> LogList(string keywords = "", int user = 0, int inviter = 0, int page = 1)
        {
            var query = db.InviteLogs.When(user > 0, i => i.UserId == user)
                .When(inviter > 0, i => i.ParentId == inviter);
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var codeId = db.InviteCodes.Where(i => i.Token == keywords)
                    .Select(i => i.Id).ToArray();
                query = query.Where(i => codeId.Contains(i.CodeId));
            }
            return query.OrderByDescending(i => i.Id).ToPage(page);
        }

        public void AddLog(InviteCodeEntity model, int userId)
        {
            model.InviteCount++;
            if (model.Amount > 0 && model.InviteCount == model.Amount)
            {
                model.ExpiredAt = client.Now - 1;
            }
            db.InviteCodes.Save(model, client.Now);
            db.SaveChanges();
            db.InviteLogs.Save(new InviteLogEntity()
            {
                UserId = userId,
                ParentId = model.UserId,
                CodeId = model.Id,
                Status = STATUS_SUCCESS
            }, client.Now);
            db.SaveChanges();
        }

        /**
         * 生成一个邀请码
         * @param int type
         * @param int amount
         * @param string|int expiredAt
         * @return string
         * @throws \Exception
         */
        public string CreateNew(byte type, int amount = 1, string expiredAt = "")
        {
            return CreateNew(type, amount, string.IsNullOrWhiteSpace(expiredAt) ? 0 : TimeHelper.TimestampFrom(expiredAt));
        }
        public string CreateNew(byte type, int amount = 1, int expiredAt = 0)
        {
            string code;
            do
            {
                code = GenerateCode(type);
            } while (HasCode(code));
            db.InviteCodes.Save(new InviteCodeEntity()
            {
                UserId = client.UserId,
                Type = type,
                Token = code,
                Amount = amount,
                ExpiredAt = expiredAt == 0 || expiredAt.ToString().Length == 10 ? expiredAt : (client.Now + expiredAt)
            }, client.Now);
            return code;
        }

        /**
         * 手动失效
         * @param int type
         * @param string token
         * @return void
         * @throws \Exception
         */
        public void Cancel(byte type, string token)
        {
            var model = db.InviteCodes.Where(i => i.Type == type && i.Token == token).Single();
            if (model is null)
            {
                return;
            }
            if (model.UserId != client.UserId)
            {
                throw new Exception("无权限操作");
            }
            model.ExpiredAt = client.Now - 1;
            db.InviteCodes.Save(model);
            db.SaveChanges();
        }

        private string GenerateCode(int type)
        {
            if (type == TYPE_CODE)
            {
                return StrHelper.RandomNumber(6);
            }
            return StrHelper.MD5Encode(StrHelper.Random(20) + DateTime.Now.ToString());
        }

        public string LoginQr(string token)
        {
            return $"auth/qr/authorize?token={token}";
        }

        /**
         * 发起人验证
         * @param int type
         * @param string token
         * @return array 返回确认的用户id
         * @throws \Exception
         */
        public IOperationResult<int> CheckQr(byte type, string token)
        {
            var model = db.InviteCodes.
                Where(i => i.Type == type && i.Token == token).Single();
            if (model is null)
            {
                return OperationResult<int>.Fail(204, "USER_TIPS_QR_OVERTIME");
            }
            if (model.InviteCount < 1)
            {
                if (IsExpired(model))
                {
                    return OperationResult<int>.Fail(204, "USER_TIPS_QR_OVERTIME");
                }
                return OperationResult<int>.Fail(201, "QR_UN_SCANNED");
            }
            if (model.Amount > 1)
            {
                return OperationResult<int>.Ok(
                    db.InviteLogs.Where(i => i.CodeId == model.Id && i.Status == STATUS_SUCCESS)
                    .Select(i => i.UserId).Single());
            }
            var log = db.InviteLogs.Where(i => i.CodeId == model.Id)
                .Single();
            if (log.Status == STATUS_UN_CONFIRM)
            {
                return OperationResult<int>.Fail(202, "QR_UN_CONFIRM");
            }
            if (log.Status != STATUS_REJECT)
            {
                return OperationResult<int>.Fail(203, "QR_REJECT");
            }
            if (log.Status != STATUS_SUCCESS)
            {
                return OperationResult<int>.Ok(0);
            }
            return OperationResult<int>.Ok(log.UserId);
        }

        private bool IsExpired(InviteCodeEntity model)
        {
            return model.ExpiredAt > 0 && model.ExpiredAt <= client.Now;
        }

        /**
         * 获取已授权的用户id
         * @param int type
         * @param string token
         * @return array
         */
        public int AuthorizedUser(byte type, string token)
        {
            var id = db.InviteCodes.Where(i => i.Type == type && i.Token == token)
                .Select(i => i.Id).Single();
            if (id == 0)
            {
                return 0;
            }
            return db.InviteLogs.Where(i => i.CodeId == id && i.Status == STATUS_SUCCESS)
                    .Select(i => i.UserId).Single();
        }
        /**
         * 接受人授权
         * @param int type
         * @param string token
         * @param bool confirm
         * @param bool reject
         * @return ?bool
         */
        public IOperationResult<bool?> Authorize(byte type, string token, bool confirm = false, bool reject = false)
        {
            if (client.UserId == 0)
            {
                return OperationResult<bool?>.Fail(204, "Need Login first");
            }
            var model = db.InviteCodes.Where(i => i.Type == type && i.Token == token)
                .Single();
            if (model is null)
            {
                return OperationResult<bool?>.Fail(204, "USER_TIPS_QR_OVERTIME");
            }
            if (IsExpired(model))
            {
                return OperationResult<bool?>.Fail(204, "USER_TIPS_QR_OVERTIME");
            }
            var userId = client.UserId;
            var log = db.InviteLogs.Where(i => i.CodeId == model.Id && i.UserId == userId)
                .Single();
            if (log is null)
            {
                if (model.InviteCount >= model.Amount)
                {
                    return OperationResult<bool?>.Fail(204, "USER_TIPS_QR_OVERTIME");
                }
                model.InviteCount++;
                db.InviteCodes.Save(model, client.Now);
                log = new InviteLogEntity()
                {
                    UserId = userId,
                    ParentId = model.UserId,
                    CodeId = model.Id,
                    Status = STATUS_UN_CONFIRM
                };
                db.InviteLogs.Save(log, client.Now);
                db.SaveChanges();
            }
            if (log.Status != STATUS_UN_CONFIRM)
            {
                return OperationResult<bool?>.Fail("二维码已失效");
            }
            if (confirm)
            {
                log.Status = STATUS_SUCCESS;
                db.InviteLogs.Save(log, client.Now);
                db.SaveChanges();
                return OperationResult<bool?>.Ok(true);
            }
            if (reject)
            {
                log.Status = STATUS_REJECT;
                db.InviteLogs.Save(log, client.Now);
                db.SaveChanges();
                return OperationResult<bool?>.Ok(false);
            }
            return OperationResult<bool?>.Ok(null);
        }
    }
}
