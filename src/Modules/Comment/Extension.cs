using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Models;
using System.Linq;

namespace NetDream.Modules.Comment
{
    public static class Extension
    {
        public static void ProvideCommentRepositories(this IServiceCollection service)
        {
        }

        internal static IQueryable<CommentListItem> SelectAs(this IQueryable<CommentEntity> query)
        {
            return query.Select(i => new CommentListItem()
            {
                Id = i.Id,
                Content = i.Content,
                AgreeCount = i.AgreeCount,
                DisagreeCount = i.DisagreeCount,
                ExtraRule = i.ExtraRule,
                ParentId = i.ParentId,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
