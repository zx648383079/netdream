using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Models;
using System.Linq;

namespace NetDream.Modules.Article
{
    public static class Extension
    {
        public static void ProvideArticleRepositories(this IServiceCollection service)
        {

        }


        internal static IQueryable<AuthorLabelItem> SelectAsLabel(this IQueryable<AuthorEntity> query)
        {
            return query.Select(i => new AuthorLabelItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Name = i.Name,
                Avatar = i.Avatar,
                Description = i.Description,
            });
        }
    }
}
