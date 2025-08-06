using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Bot.Adapters;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Models;
using NetDream.Modules.Bot.Repositories;
using System;
using System.Linq;

namespace NetDream.Modules.Bot
{
    public static class Extension
    {
        public static void ProvideBotRepositories(this IServiceCollection service)
        {
            service.AddScoped<BotRepository>();
        }

        internal static IQueryable<BotListItem> SelectAs(this IQueryable<BotEntity> query)
        {
            return query.Select(i => new BotListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar,
                Qrcode = i.Qrcode,
                Status = i.Status,
                UpdatedAt = i.UpdatedAt,
                UserId = i.UserId,
                Description = i.Description,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<MediaListItem> SelectAs(this IQueryable<MediaEntity> query)
        {
            return query.Select(i => new MediaListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Type = i.Type,
                MediaId = i.MediaId,
                Thumb = i.Thumb,
                ParentId = i.ParentId,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<UserListItem> SelectAs(this IQueryable<UserEntity> query)
        {
            return query.Select(i => new UserListItem()
            {
                Id = i.Id,
                Nickname = i.Nickname,
                NoteName = i.NoteName,
                Avatar = i.Avatar,
                Status = i.Status,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt
            });
        }

        internal static IQueryable<UserLabelItem> SelectAsLabel(this IQueryable<UserEntity> query)
        {
            return query.Select(i => new UserLabelItem()
            {
                Id = i.Id,
                Nickname = i.Nickname,
                NoteName = i.NoteName,
            });
        }

        internal static IQueryable<HistoryListItem> SelectAs(this IQueryable<MessageHistoryEntity> query)
        {
            return query.Select(i => new HistoryListItem()
            {
                Id = i.Id,
                From = i.From,
                To = i.To,
                BotId = i.BotId,
                Content = i.Content,
                IsMark = i.IsMark,
                ItemId = i.ItemId,
                ItemType = i.ItemType,
                Type = i.Type,
                CreatedAt = i.CreatedAt
            });
        }

        internal static string ToEventName(this AdapterEvent source)
        {
            return Enum.GetName(source)!.ToLower();
        }
    }
}
