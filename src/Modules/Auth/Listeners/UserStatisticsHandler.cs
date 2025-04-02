using MediatR;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Auth.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class UserStatisticsHandler(AuthContext db) : INotificationHandler<UserStatistics>
    {

        public Task Handle(UserStatistics request, CancellationToken cancellationToken)
        {
            if (request.IsInclude("bulletin_count"))
            {
                request.Add(new("未读消息", BulletinRepository.UnReadCount(db, request.UserId), "条"));
            }
            if (request.IsInclude("following_count"))
            {
                request.Add(new("已关注", db.Relationships.Where(i => i.UserId == request.UserId && i.Type == RelationshipRepository.TYPE_FOLLOWING).Count()));
            }
            if (request.IsInclude("follower_count"))
            {
                request.Add(new("被关注", db.Relationships.Where(i => i.LinkId == request.UserId && i.Type == RelationshipRepository.TYPE_FOLLOWING).Count()));
            }
            return Task.CompletedTask;
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
