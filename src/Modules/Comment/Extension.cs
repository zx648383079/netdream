using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Comment.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Comment
{
    public static class Extension
    {
        public static void ProvideCommentRepositories(this IServiceCollection service)
        {
            service.AddScoped<ICommentRepository, CommentRepository>();
        }
    }
}
