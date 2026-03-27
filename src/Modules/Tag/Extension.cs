using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Tag.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Tag
{
    public static class Extension
    {
        public static void ProvideTagRepositories(this IServiceCollection service)
        {
            service.AddScoped<ITagRepository, TagRepository>();
        }
    }
}
