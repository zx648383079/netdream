using MediatR;
using NetDream.Shared.Notifications;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.MicroBlog.Listeners
{
    public class UserStatisticsHandler(MicroBlogContext db) : INotificationHandler<UserStatisticsRequest>
    {
        public Task Handle(UserStatisticsRequest request, CancellationToken cancellationToken)
        {
            if (request.IsInclude("micro_count"))
            {
                request.Add(new("微博文数量", db.Blogs.Where(i => i.UserId == request.UserId).Count(), "篇"));
                request.Add(new("微博文评论", db.Comments.Where(i => i.UserId == request.UserId).Count(), "条"));
            }
            return Task.CompletedTask;
        }
    }
}
