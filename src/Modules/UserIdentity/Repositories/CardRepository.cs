using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.UserIdentity.Repositories
{
    public class CardRepository(IdentityContext db, IClientContext client)
    {
        /// <summary>
        /// 过期的
        /// </summary>
        public const byte STATUS_EXPIRED = 0;
        /// <summary>
        /// 有效激活的
        /// </summary>
        public const byte STATUS_ACTIVATED = 1;

        public IPage<EquityCardListItem> GetList(QueryForm form)
        {
            var items = db.EquityCards.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            WithAmount(items.Items);
            return items;
        }

        private void WithAmount(IEnumerable<EquityCardListItem> items)
        {
            var idItems = items.Select(item => item.Id);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.UserEquityCards.Where(i => idItems.Contains(i.CardId))
                .GroupBy(i => i.CardId)
                .Select(i => new { CardId = i.Key, Count = i.Count() })
                .ToDictionary(i => i.CardId, i => i.Count);
            if (data is null || data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (data.TryGetValue(item.Id, out var count))
                {
                    item.Amount = count;
                }
            }
        }

        public IOperationResult<EquityCardEntity> Save(EquityCardForm data)
        {
            var model = data.Id > 0 ? db.EquityCards.Where(i => i.Id == data.Id).Single() : 
                new EquityCardListItem();
            if (model is null)
            {
                return OperationResult.Fail<EquityCardEntity>("id is error");
            }
            model.Name = data.Name;
            model.Icon = data.Icon;
            model.Configure = data.Configure;
            db.EquityCards.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.EquityCards.Where(i => i.Id == id).ExecuteDelete();
            db.UserEquityCards.Where(i => i.CardId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<UserEquityCardEntity> UserCardList(int user, QueryForm form)
        {
            return db.UserEquityCards
                .Include(i => i.Card).Where(i => i.UserId == user)
                .OrderByDescending(i => i.ExpiredAt)
                .ToPage(form);
        }


        /// <summary>
        /// 续期权益卡
        /// </summary>
        /// <param name="user"></param>
        /// <param name="card"></param>
        /// <param name="second"></param>
        public void Recharge(int user, int card, int second)
        {
            var model = db.EquityCards.Where(i => i.Id == card).Single();
            var log = db.UserEquityCards.Where(
                i => i.CardId == card && i.UserId == user).Single();
            var now = TimeHelper.TimestampNow();
            if (log is null)
            {
                db.UserEquityCards.Save(new UserEquityCardEntity()
                {
                    UserId = user,
                    CardId = card,
                    Status = 1,
                    ExpiredAt = now + second
                }, now);
            } else
            {
                log.ExpiredAt = Math.Max(log.ExpiredAt, now) + second;
                log.Status = 1;
                db.UserEquityCards.Save(log, now);
            }
            db.SaveChanges();
        }

        public IPage<EquityCardLabelItem> Search(QueryForm form, int[] idItems)
        {
            return db.EquityCards.Search(form.Keywords, "name")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .OrderByDescending(i => i.Id)
                .Select(i => new EquityCardLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Icon = i.Icon
                }).ToPage(form);
        }

        public IOperationResult<UserEquityCardEntity> UserUpdate(BindingCardForm data)
        {
            return UserUpdate(data.User, data.Card, TimeHelper.TimestampFrom(data.ExpiredAt));
        }
        public IOperationResult<UserEquityCardEntity> UserUpdate(int userId, int cardId, string expiredAt)
        {
            return UserUpdate(userId, cardId, TimeHelper.TimestampFrom(expiredAt));
        }
        public IOperationResult<UserEquityCardEntity> UserUpdate(int userId, int cardId, int expiredAt)
        {
            if (!db.EquityCards.Where(i => i.Id == cardId).Any())
            {
                return OperationResult<UserEquityCardEntity>.Fail("数据错误");
            }
            var log = db.UserEquityCards.Where(i => i.CardId == cardId && i.UserId == userId).SingleOrDefault();
            var status = expiredAt > TimeHelper.TimestampNow() ? STATUS_ACTIVATED : STATUS_EXPIRED;
            if (log is null)
            {
                log = new UserEquityCardEntity()
                {
                    UserId = userId,
                    CardId = cardId,
                    Status = status,
                    ExpiredAt = expiredAt
                };
                
            } else
            {
                log.ExpiredAt = expiredAt;
                log.Status = status;
            }
            db.UserEquityCards.Save(log);
            db.SaveChanges();
            // TODO 埋点记录管理账户记录
            return OperationResult.Ok(log);
        }

        public static UserCardItem[] GetUserCard(IdentityContext db, int user)
        {
            var now = TimeHelper.TimestampNow();
            var items = db.UserEquityCards
                .Include(i => i.Card)
                .Where(i => i.UserId == user && i.Status == STATUS_ACTIVATED && i.ExpiredAt > now)
                .OrderByDescending(i => i.CardId).ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            var data = new List<UserCardItem>();
            foreach (var item in items)
            {
                if (item.Card is null)
                {
                    continue;
                }
                data.Add(new()
                {
                    Id = item.CardId,
                    Status = item.Status,
                    ExpiredAt = TimeHelper.TimestampTo(item.ExpiredAt),
                    Exp = item.Exp,
                    Name = item.Card.Name,
                    Icon = item.Card.Icon,
                });
            }
            return [..data];
        }
    }
}
