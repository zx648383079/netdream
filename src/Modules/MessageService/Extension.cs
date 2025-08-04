using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Models;
using NetDream.Modules.MessageService.Repositories;
using System.Linq;

namespace NetDream.Modules.MessageService
{
    public static class Extension
    {
        public static void ProvideMessageRepositories(this IServiceCollection service)
        {
            service.AddScoped<MessageProtocol>();
            service.AddScoped<MessageRepository>();
        }

        internal static IQueryable<LogListItem> SelectAs(this IQueryable<LogEntity> query)
        {
            return query.Select(i => new LogListItem()
            {
                Id = i.Id,
                Target = i.Target,
                TemplateName = i.TemplateName,
                Status = i.Status,
                Message = i.Message,
                CreatedAt = i.CreatedAt,
            });
        }

        internal static IQueryable<TemplateListItem> SelectAs(this IQueryable<TemplateEntity> query)
        {
            return query.Select(i => new TemplateListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Type = i.Type,
                TargetNo = i.TargetNo,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
