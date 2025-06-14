using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class ChatRepository(
        ChatContext db, 
        IUserRepository userStore,
        IClientContext client)
    {
        public IPage<HistoryModel> Histories(int page)
        {
            var items = db.Histories.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt).ToPage(page)
                .CopyTo<HistoryEntity, HistoryModel>();
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
            db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id)
                .ExecuteDelete();
        }

        public void RemoveIdHistory(int id)
        {
            db.Histories.Where(i => i.Id == id && i.UserId == client.UserId)
                .ExecuteDelete();
        }

        public void ClearUnread(int type, int id, int user_id)
        {
            db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id)
               .ExecuteUpdate(setters => setters.SetProperty(i => i.UnreadCount, 0));
        }

        protected void AddIfHistory(int type, int id, int user_id, int message = 0, int messageCount = 0)
        {
            var count = db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id).Any();
            if (count)
            {
                db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id)
                    .ExecuteUpdate(setters => 
                    setters.SetProperty(i => i.UnreadCount, i => i.UnreadCount + messageCount)
                    .SetProperty(i => i.LastMessage, message));
                return;
            }
            db.Histories.Save(new HistoryEntity()
            {
                ItemType = type,
                ItemId = id,
                UserId = user_id,
                UnreadCount = messageCount,
                LastMessage = message
            }, client.Now);
        }

        protected Dictionary<int, MessageEntity> GetLastMessage(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Messages.Where(i => ids.Contains(i.Id)).ToDictionary(i => i.Id);
        }

        protected Dictionary<int, GroupEntity> GetGroup(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Groups.Where(i => ids.Contains(i.Id)).ToDictionary(i => i.Id);
        }

        protected Dictionary<int, FriendEntity> GetFriend(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Friends.Where(i => ids.Contains(i.UserId) && i.BelongId == client.UserId).ToDictionary(i => i.UserId);
        }

        protected Dictionary<int, IUserSource> GetUsers(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return userStore.Get(ids).ToDictionary(i => i.Id);
        }
    }
}
