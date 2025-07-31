using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.OpenPlatform.Repositories;
using System.Linq;

namespace NetDream.Modules.OpenPlatform
{
    public static class Extension
    {
        public static void ProvideOpenRepositories(this IServiceCollection service)
        {
            service.AddScoped<OpenRepository>();
        }

        internal static IQueryable<PlatformListItem> SelectAs(this IQueryable<PlatformEntity> query)
        {
            return query.Select(i => new PlatformListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Domain = i.Domain,
                Appid = i.Appid,
                Type = i.Type,
                Status = i.Status,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<TokenListItem> SelectAs(this IQueryable<UserTokenEntity> query)
        {
            return query.Select(i => new TokenListItem()
            {
                Id = i.Id,
                PlatformId = i.PlatformId,
                ExpiredAt = i.ExpiredAt,
                IsSelf = i.IsSelf,
                Token = i.Token,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }
    }
}
