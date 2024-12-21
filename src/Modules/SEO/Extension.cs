using Microsoft.Extensions.DependencyInjection;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using NetDream.Modules.SEO.Repositories;
using NetDream.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace NetDream.Modules.SEO
{
    public static class Extension
    {
        public static void ProvideSEORepositories(this IServiceCollection service, DbContextOptions<SEOContext> options)
        {
            using var context = new SEOContext(options);
            var option = new OptionRepository(context);
            service.AddSingleton(typeof(IGlobeOption), option.LoadOption());
            service.AddScoped<OptionRepository>();
            service.AddScoped<LocalizeRepository>();
            service.AddScoped<IDeeplink, Deeplink>();
            service.AddScoped<ILinkRuler, LinkRuler>();
        }
    }
}
