using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Models;
using NetDream.Modules.OnlineService.Repositories;
using System.Linq;

namespace NetDream.Modules.OnlineService
{
    public static class Extension
    {
        public static void ProvideOnlineServiceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ChatRepository>();
            service.AddScoped<CategoryRepository>();
            service.AddScoped<SessionRepository>();
        }

        internal static IQueryable<CategoryUserListItem> SelectAs(this IQueryable<CategoryUserEntity> query)
        {
            return query.Select(i => new CategoryUserListItem()
            {
                Id = i.Id,
                CatId = i.CatId,
                UserId = i.UserId,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<MessageListItem> SelectAs(this IQueryable<MessageEntity> query)
        {
            return query.Select(i => new MessageListItem()
            {
                Id = i.Id,
                Content = i.Content,
                ExtraRule = i.ExtraRule,
                SendType = i.SendType,
                SessionId = i.SessionId,
                Status = i.Status,
                Type = i.Type,
                UserId = i.UserId,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<SessionListItem> SelectAs(this IQueryable<SessionEntity> query)
        {
            return query.Select(i => new SessionListItem()
            {
                Id = i.Id,
                Ip = i.Ip,
                ServiceId = i.ServiceId,
                Name = i.Name,
                Remark = i.Remark,
                Status = i.Status,
                ServiceWord = i.ServiceWord,
                UserId = i.UserId,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
