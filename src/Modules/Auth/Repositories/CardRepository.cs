using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Repositories
{
    public class CardRepository(IDatabase db, IClientContext client)
    {
        public Page<EquityCardModel> GetList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<EquityCardEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            sql.OrderBy("id DESC");
            var items = db.Page<EquityCardModel>(page, 20, sql);
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
            var sql = new Sql();
            sql.Select("card_id, COUNT(user_id) as count").From<UserEquityCardEntity>(db)
                .WhereIn("card_id", [..idItems])
                .GroupBy("card_id");
            var data = db.Pluck<int, int>(sql, "card_id", "count", false);
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

        public EquityCardEntity Save(EquityCardForm data)
        {
            var model = data.Id > 0 ? db.SingleById<EquityCardEntity>(data.Id) : 
                new EquityCardModel();
            model.Name = data.Name;
            model.Icon = data.Icon;
            model.Configure = data.Configure;
            if (!db.TrySave(model))
            {
                throw new Exception("save error");
            }
            return model;
        }

        public void Remove(int id)
        {
            db.DeleteById<EquityCardEntity>(id);
            db.DeleteWhere<UserEquityCardEntity>("card_id=@0", id);
        }

        public Page<UserEquityCardModel> UserCardList(int user, int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<UserEquityCardEntity>(db);
            sql.Where("user_id=@0", user).OrderBy("expired_at DESC");
            var items = db.Page<UserEquityCardModel>(page, 20, sql);
            WithCard(items.Items);
            return items;
        }

        private void WithCard(IEnumerable<UserEquityCardModel> items)
        {
            var idItems = items.Select(item => item.CardId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<EquityCardEntity>($"WHERE id IN({string.Join(',', idItems)})");
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.CardId == it.Id)
                    {
                        item.Card = it;
                        break;
                    }
                }
            }
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
            var model = db.SingleById<EquityCardEntity>(card);
            var log = db.First<UserEquityCardEntity>("card_id=@0 and user_id=@1", card, user);
            var now = TimeHelper.TimestampNow();
            if (log is null)
            {
                db.Insert(new UserEquityCardEntity()
                {
                    UserId = user,
                    CardId = card,
                    Status = 1,
                    ExpiredAt = now + second
                });
                return;
            }
            log.ExpiredAt = Math.Max(log.ExpiredAt, now) + second;
            log.Status = 1;
            db.Update(log);
        }

        public Page<EquityCardEntity> Search(string keywords, int[] id, int page = 1)
        {
            var sql = new Sql();
            sql.Select("id", "name", "icon").From<EquityCardEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            if (id.Length > 0)
            {
                sql.WhereIn("id", id);
            }
            sql.OrderBy("id DESC");
            var items = db.Page<EquityCardEntity>(page, 20, sql);
            return items;
        }
        public UserEquityCardEntity UserUpdate(int userId, int cardId, string expiredAt)
        {
            return UserUpdate(userId, cardId, TimeHelper.TimestampFrom(expiredAt));
        }
        public UserEquityCardEntity UserUpdate(int userId, int cardId, int expiredAt)
        {
            var log = db.First<UserEquityCardEntity>("card_id=@0 and user_id=@1", cardId, userId);
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
                db.Insert(log);
                return log;
            }
            log.ExpiredAt = expiredAt;
            log.Status = status;
            db.Update(log);
            // TODO 埋点记录管理账户记录
            return log;
        }

        public UserEquityCard[] GetUserCard(int user)
        {
            var sql = new Sql();
            sql.Select().From<UserEquityCardEntity>(db);
            sql.Where("user_id=@0 and status=1 and expired_at>@1", user, TimeHelper.TimestampNow()).OrderBy("card_id DESC");
            var items = db.Fetch<UserEquityCardModel>(sql);
            if (items.Count == 0)
            {
                return [];
            }
            WithCard(items);
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
