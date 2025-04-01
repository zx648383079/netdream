using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class CardRepository(AuthContext db, IClientContext client)
    {
        public IPage<EquityCardModel> GetList(string keywords = "", int page = 1)
        {
            var items = db.EquityCards.Search(keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(page).CopyTo<EquityCardEntity, EquityCardModel>();
            WithAmount(items.Items);
            return items;
        }

        private void WithAmount(IEnumerable<EquityCardModel> items)
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
                new EquityCardModel();
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
        }

        public IPage<UserEquityCardEntity> UserCardList(int user, int page = 1)
        {
            return db.UserEquityCards
                .Include(i => i.Card).Where(i => i.UserId == user)
                .OrderByDescending(i => i.ExpiredAt)
                .ToPage(page);
        }


        /**
         * 续期权益卡
         * @param int user
         * @param int card
         * @param int second
         * @return void
         * @throws Exception
         */
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

        public IPage<EquityCardEntity> Search(string keywords, int[] id, int page = 1)
        {
            return db.EquityCards.Search(keywords, "name")
                .When(id.Length > 0, i => id.Contains(i.Id))
                .OrderByDescending(i => i.Id)
                .Select(i => new EquityCardEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Icon = i.Icon
                }).ToPage(page);
        }
        public UserEquityCardEntity UserUpdate(int userId, int cardId, string expiredAt)
        {
            return UserUpdate(userId, cardId, TimeHelper.TimestampFrom(expiredAt));
        }
        public UserEquityCardEntity UserUpdate(int userId, int cardId, int expiredAt)
        {
            var log = db.UserEquityCards.Where(i => i.CardId == cardId && i.UserId == userId).Single();
            var status = expiredAt > TimeHelper.TimestampNow() ? 1 : 0;
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
            return log;
        }

        public static UserEquityCard[] GetUserCard(AuthContext db, int user)
        {
            var now = TimeHelper.TimestampNow();
            var items = db.UserEquityCards
                .Include(i => i.Card)
                .Where(i => i.UserId == user && i.Status == 1 && i.ExpiredAt > now)
                .OrderByDescending(i => i.CardId).ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            var data = new List<UserEquityCard>();
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
                    ExpiredAt = TimeHelper.Format(item.ExpiredAt),
                    Exp = item.Exp,
                    Name = item.Card.Name,
                    Icon = item.Card.Icon,
                });
            }
            return [..data];
        }
    }
}
