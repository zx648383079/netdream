using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.MicroBlog.Listeners
{
    public class UserStatisticsHandler(MicroBlogContext db, ICommentRepository comment) : INotificationHandler<UserStatisticsRequest>
    {
        public Task Handle(UserStatisticsRequest request, CancellationToken cancellationToken)
        {
            if (request.IsInclude("micro_count"))
            {
                request.Add(new("微博文数量", db.Blogs.Where(i => i.UserId == request.UserId).Count(), "篇"));
                request.Add(new("微博文评论", comment.Count(request.UserId, ModuleTargetType.MicroBlog), "条"));
            }
            return Task.CompletedTask;
        }
    }
}
