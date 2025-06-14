using MediatR;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Notifications;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Listeners
{
    public class UserStatisticsHandler(BlogContext db) : INotificationHandler<UserStatistics>
    {

        public Task Handle(UserStatistics request, CancellationToken cancellationToken)
        {
            request.Add(new("博文数量", db.Blogs.Where(i => i.UserId == request.UserId).Count(), "篇"));
            request.Add(new("博文评论", db.Comments.Where(i => i.UserId == request.UserId).Count(), "条"));
            return Task.CompletedTask;
        }
    }
    public class UserOpenStatisticsHandler(BlogContext db) : INotificationHandler<UserOpenStatistics>
    {

        public Task Handle(UserOpenStatistics request, CancellationToken cancellationToken)
        {
            request.TryAdd("post_count", () => BlogRepository.PublishCount(db, request.UserId));
            return Task.CompletedTask;
        }
    }
}
