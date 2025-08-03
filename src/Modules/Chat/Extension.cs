using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Models;
using NetDream.Modules.Chat.Repositories;
using NetDream.Shared.Models;
using System.Linq;

namespace NetDream.Modules.Chat
{
    public static class Extension
    {
        public static void ProvideChatRepositories(this IServiceCollection service)
        {
            service.AddScoped<ChatRepository>();
        }

        internal static IQueryable<ListLabelItem> SelectAs(this IQueryable<FriendClassifyEntity> query)
        {
            return query.Select(i => new ListLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
            });
        }

        internal static IQueryable<ApplyListItem> SelectAs(this IQueryable<ApplyEntity> query)
        {
            return query.Select(i => new ApplyListItem()
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemType = i.ItemType,
                Remark = i.Remark,
                Status = i.Status,
                UserId = i.UserId,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
        internal static IQueryable<HistoryListItem> SelectAs(this IQueryable<HistoryEntity> query)
        {
            return query.Select(i => new HistoryListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                UnreadCount = i.UnreadCount,
                LastMessage = i.LastMessage,
                ItemId = i.ItemId,
                ItemType = i.ItemType,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<FriendListItem> SelectAs(this IQueryable<FriendEntity> query)
        {
            return query.Select(i => new FriendListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                BelongId = i.BelongId,
                ClassifyId = i.ClassifyId,
                Name = i.Name,
                Status = i.Status,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<FriendLabelItem> SelectAsLabel(this IQueryable<FriendEntity> query)
        {
            return query.Select(i => new FriendLabelItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Name = i.Name,
            });
        }

        internal static IQueryable<MessageLabelItem> SelectAsLabel(this IQueryable<MessageEntity> query)
        {
            return query.Select(i => new MessageLabelItem()
            {
                Id = i.Id,
                Type = i.Type,
                Content = i.Content,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<MessageListItem> SelectAs(this IQueryable<MessageEntity> query)
        {
            return query.Select(i => new MessageListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Type = i.Type,
                Content = i.Content,
                ItemId = i.ItemId,
                GroupId = i.GroupId,
                ReceiveId = i.ReceiveId,
                ExtraRule = i.ExtraRule,
                Status = i.Status,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<GroupLabelItem> SelectAsLabel(this IQueryable<GroupEntity> query)
        {
            return query.Select(i => new GroupLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
                Logo = i.Logo,
            });
        }

        internal static IQueryable<GroupListItem> SelectAs(this IQueryable<GroupEntity> query)
        {
            return query.Select(i => new GroupListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Logo = i.Logo,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }
    }
}
