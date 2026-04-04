using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Chat.Repositories
{
    public class ChatRepository(
        ChatContext db, 
        IUserRepository userStore,
        IClientContext client,
        ITeamRepository teamStore)
    {
        public IPage<HistoryListItem> Histories(QueryForm form)
        {
            var items = db.Histories.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
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
                item.Message = messages.TryGetValue(item.LastMessage, out var ms) ? ms : null;
                if (item.ItemType < 1)
                {
                    item.User = users.TryGetValue(item.ItemId, out var u) ? u : null;
                    item.Friend = friends.TryGetValue(item.ItemId, out var f) ? f : null;
                    continue;
                }
                item.Group = groups.TryGetValue(item.ItemId, out var g) ? g : null;
            }
            return items;
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

        
        protected Dictionary<int, MessageLabelItem> GetLastMessage(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Messages.Where(i => ids.Contains(i.Id)).SelectAsLabel().ToDictionary(i => i.Id);
        }

        protected Dictionary<int, IListLabelItem> GetGroup(params int[] ids)
        {
            return teamStore.Get(ids).ToDictionary(i => i.Id);
        }

        protected Dictionary<int, FriendLabelItem> GetFriend(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return db.Friends.Where(i => ids.Contains(i.UserId) && i.BelongId == client.UserId)
                .SelectAsLabel().ToDictionary(i => i.UserId);
        }

        protected Dictionary<int, IUserSource> GetUsers(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return [];
            }
            return userStore.Get(ids).ToDictionary(i => i.Id);
        }

        public UserProfileModel GetProfile()
        {
            return new UserProfileModel(userStore.Get(client.UserId))
            {
                NewCount = db.Messages.Where(i => i.ReceiveId == client.UserId && i.Status == 0).Count()
            };
        }
    }
}
