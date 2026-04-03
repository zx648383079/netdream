using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Article.Listeners
{
    public class UserStatisticsHandler(ArticleContext db, ICommentRepository comment) : INotificationHandler<UserStatisticsRequest>
    {

        public Task Handle(UserStatisticsRequest request, CancellationToken cancellationToken)
        {
            request.Add(new("博文数量", db.Articles.Where(i => i.UserId == request.UserId).Count(), "篇"));
            request.Add(new("博文评论", comment.Count(request.UserId, ModuleTargetType.Article), "条"));
            return Task.CompletedTask;
        }
    }
    public class UserOpenStatisticsHandler(ArticleContext db) : INotificationHandler<UserOpenStatisticsRequest>
    {

        public Task Handle(UserOpenStatisticsRequest request, CancellationToken cancellationToken)
        {
            request.TryAdd("post_count",() => db.Articles.Where(i => i.UserId == request.UserId &&
                i.ParentId == 0 && i.PublishStatus == (byte)PublishStatus.Posted)
                .Count());
            return Task.CompletedTask;
        }
    }
}
