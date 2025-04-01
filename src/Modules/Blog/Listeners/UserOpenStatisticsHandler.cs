using MediatR;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Listeners
{
    public class UserStatisticsHandler(BlogContext db) : IRequestHandler<UserStatistics, IEnumerable<StatisticsItem>>
    {

        public Task<IEnumerable<StatisticsItem>> Handle(UserStatistics request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<StatisticsItem>>([
                new("博文数量", db.Blogs.Where(i => i.UserId == request.UserId).Count(), "篇"),
                new("博文评论", db.Comments.Where(i => i.UserId == request.UserId).Count(), "条"),
            ]);
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
