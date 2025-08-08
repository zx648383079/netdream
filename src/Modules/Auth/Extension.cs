using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Models;
using System.Linq;

namespace NetDream.Modules.Auth
{
    public static class Extension
    {
        public static void ProvideAuthRepositories(this IServiceCollection service)
        {
            service.AddScoped<AuthRepository>();
            service.AddScoped<CaptchaRepository>();
        }

        internal static IQueryable<InviteLogListItem> SelectAs(this IQueryable<InviteLogEntity> query)
        {
            return query.Select(i => new InviteLogListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                ParentId = i.ParentId,
                CodeId = i.CodeId,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
