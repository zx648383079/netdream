using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Modules.Blog
{
    public static class Extension
    {
        public static void ProvideBlogRepositories(this IServiceCollection service)
        {
            service.AddScoped<BlogRepository>();
        }
    }
}
