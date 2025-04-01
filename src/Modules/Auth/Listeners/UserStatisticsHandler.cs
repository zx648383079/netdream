using MediatR;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Auth.Repositories;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class UserStatisticsHandler(AuthContext db, IEnumerable<IRequestHandler<UserStatistics, IEnumerable<StatisticsItem>>> items) : IRequestHandler<UserStatistics, IEnumerable<StatisticsItem>>
    {

        public async Task<IEnumerable<StatisticsItem>> Handle(UserStatistics request, CancellationToken cancellationToken)
        {
            var res = new List<StatisticsItem>();
            if (request.IsInclude("bulletin_count"))
            {
                res.Add(new("未读消息", BulletinRepository.UnReadCount(db, request.UserId), "条"));
            }
            if (request.IsInclude("following_count"))
            {
                res.Add(new("已关注", db.Relationships.Where(i => i.UserId == request.UserId && i.Type == RelationshipRepository.TYPE_FOLLOWING).Count()));
            }
            if (request.IsInclude("follower_count"))
            {
                res.Add(new("被关注", db.Relationships.Where(i => i.LinkId == request.UserId && i.Type == RelationshipRepository.TYPE_FOLLOWING).Count()));
            }
            foreach (var item in items)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (item == this)
                {
                    continue;
                }
                res.AddRange(await item.Handle(request, cancellationToken));
            }
            return [.. res];
        }
    }

    public class UserOpenStatisticsHandler(AuthContext db) : INotificationHandler<UserOpenStatistics>
    {

        public Task Handle(UserOpenStatistics request, CancellationToken cancellationToken)
        {
            request.TryAdd("bulletin_count", () => BulletinRepository.UnReadCount(db, request.UserId));
            request.TryAdd("following_count", () => RelationshipRepository.FollowingCount(db, request.UserId));
            request.TryAdd("follower_count", () => RelationshipRepository.FollowerCount(db, request.UserId));
            request.TryAdd("last_ip", () => UserRepository.GetLastIp(db, request.UserId));
            request.TryAdd("card_items", () => CardRepository.GetUserCard(db, request.UserId));
            return Task.CompletedTask;
        }
    }
}
