using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Document.Repositories;

namespace NetDream.Modules.Document
{
    public static class Extension
    {
        public static void ProvideDocumentRepositories(this IServiceCollection service)
        {
            service.AddScoped<PageRepository>();
        }
    }
}
