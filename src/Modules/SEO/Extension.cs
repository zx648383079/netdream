using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Models;
using NetDream.Modules.SEO.Repositories;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using System.Linq;

namespace NetDream.Modules.SEO
{
    public static class Extension
    {
        public static void ProvideSEORepositories(this IServiceCollection service, DbContextOptions<SEOContext> options)
        {
            using (var context = new SEOContext(options))
            {
                var option = new OptionRepository(context);
                service.AddSingleton(typeof(IGlobeOption), option.LoadOption());
            }
            service.AddScoped<OptionRepository>();
            service.AddScoped<LocalizeRepository>();
            service.AddScoped<IDeeplink, Deeplink>();
            service.AddScoped<ILinkRuler, LinkRuler>();
            service.AddScoped<AgreementRepository>();
        }

        internal static IQueryable<AgreementListItem> SelectAs(this IQueryable<AgreementEntity> query)
        {
            return query.Select(i => new AgreementListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Language = i.Language,
                Status  = i.Status,
                Title = i.Title,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
