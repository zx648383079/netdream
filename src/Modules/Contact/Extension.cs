using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Models;
using NetDream.Modules.Contact.Repositories;
using NetDream.Shared.Interfaces;
using System.Linq;

namespace NetDream.Modules.Contact
{
    public static class Extension
    {
        public static void ProvideContactRepositories(this IServiceCollection service)
        {
            service.AddScoped<ContactRepository>();
            service.AddScoped<FeedbackRepository>();
            service.AddScoped<FriendLinkRepository>();
            service.AddScoped<SubscribeRepository>();
            service.AddScoped<ReportRepository>();
            service.AddScoped<ISystemFeedback, ReportRepository>();
        }

        internal static IQueryable<ReportListItem> SelectAs(this IQueryable<ReportEntity> query)
        {
            return query.Select(i => new ReportListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Content = i.Content,
                Email = i.Email,
                Ip = i.Ip,
                Files = i.Files,
                ItemId = i.ItemId,
                ItemType = i.ItemType,
                Status = i.Status,
                Title = i.Title,
                Type = i.Type,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
