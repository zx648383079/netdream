using Google.Protobuf;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Repositories
{
    public class ChatRepository(
        IDatabase db, 
        IUserRepository userStore,
        IClientEnvironment environment)
    {
        public Page<HistoryModel> Histories(int page)
        {
            var items = db.Page<HistoryModel>(page, 20, "WHERE user_id=@0 ORDER BY updated_at desc", 
                environment.UserId);
            var userIds = new List<int>();
            var groupIds = new List<int>();
            var messageIds = new List<int>();
            foreach (var item in items.Items)
            {
                if (item.LastMessage > 0)
                {
                    messageIds.Add(item.LastMessage);
                }
                if (item.ItemType < 1)
                {
                    userIds.Add(item.ItemId);
                    continue;
                }
                groupIds.Add(item.ItemId);
            }
            var users = GetUsers([..userIds]);
            var friends = GetFriend([..userIds]);
            var groups = GetGroup([..groupIds]);
            var messages = GetLastMessage([..messageIds]);
            foreach (var item in items.Items)
            {
                item.Message = messages[item.LastMessage] ?? null;
                if (item.ItemType < 1)
                {
                    item.User = users[item.ItemId] ?? null;
                    item.Friend = friends[item.ItemId] ?? null;
                    continue;
                }
                item.Group = groups[item.ItemId] ?? null;
            }
            return items;
        }

        public void AddHistory(int type, int id, int user_id, int message = 0, int count = 0)
        {
            AddIfHistory(type, id, user_id, message, 0);
            if (type > 0)
            {
                return;
            }
            AddIfHistory(type, user_id, id, message, count < 1 ? 1 : count);
        }

        public void RemoveHistory(int type, int id, int user_id)
        {
            db.DeleteWhere<HistoryEntity>("item_type=@0 and item_id=@1 and user_id=@2",
                type, id, user_id);
        }

        public void RemoveIdHistory(int id)
        {
            db.DeleteWhere<HistoryEntity>("id=@0 and user_id=@1",
                id, environment.UserId);
        }

        public void ClearUnread(int type, int id, int user_id)
        {
            db.Update<HistoryEntity>("SET unread_count=0 WHERE item_type=@0 and item_id=@1 and user_id=@2",
                type, id, user_id);
        }

        protected void AddIfHistory(int type, int id, int user_id, int message = 0, int messageCount = 0)
        {
            var count = db.FindCount<HistoryEntity>("item_type=@0 and item_id=@1 and user_id=@2",
                type, id, user_id);
            if (count > 0)
            {
                db.UpdateWhere<HistoryEntity>("unread_count=unread_count+@0, last_message=@1",
                    "item_type=@2 and item_id=@3 and user_id=@4",
                    messageCount, message, type, id, user_id);
                return;
            }
            db.Insert(new HistoryEntity()
            {
                ItemType = type,
                ItemId = id,
                UserId = user_id,
                UnreadCount = messageCount,
                LastMessage = message
            });
        }

        protected Dictionary<int, MessageEntity> GetLastMessage(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            var sql = new Sql();
            sql.Select().From<MessageEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<MessageEntity>(sql).ToDictionary(i => i.Id);
        }

        protected Dictionary<int, GroupEntity> GetGroup(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            var sql = new Sql();
            sql.Select().From<GroupEntity>(db)
                .WhereIn("id", ids);
            return db.Fetch<GroupEntity>(sql).ToDictionary(i => i.Id);
        }

        protected Dictionary<int, FriendEntity> GetFriend(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            var sql = new Sql();
            sql.Select().From<FriendEntity>(db)
                .WhereIn("user_id", ids)
                .Where("belong_id=@0", environment.UserId);
            return db.Fetch<FriendEntity>(sql).ToDictionary(i => i.UserId);
        }

        protected Dictionary<int, IUser> GetUsers(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return userStore.Get(ids).ToDictionary(i => i.Id);
        }
    }
}
