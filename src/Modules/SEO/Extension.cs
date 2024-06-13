using Microsoft.Extensions.DependencyInjection;
using NetDream.Core.Interfaces;
using NetDream.Modules.SEO.Repositories;
using NPoco;

namespace NetDream.Modules.SEO
{
    public static class Extension
    {
        public static void ProvideSEORepositories(this IServiceCollection service, IDatabase db)
        {
            var option = new OptionRepository(db);
            service.AddSingleton(typeof(IGlobeOption), option.LoadOption());
        }
    }
}
