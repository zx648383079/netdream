using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Book.Repositories;

namespace NetDream.Modules.Book
{
    public static class Extension
    {
        public static void ProvideBookRepositories(this IServiceCollection service)
        {
            service.AddScoped<CategoryRepository>();
        }
    }
}
