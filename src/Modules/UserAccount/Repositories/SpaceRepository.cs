using MediatR;
using NetDream.Modules.UserAccount.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Repositories;
using System;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class SpaceRepository(UserContext db, 
        IClientContext client, 
        UserRepository repository,
        IMediator mediator)
    {
        public IOperationResult<UserProfile> Get(int userId)
        {
            var user = repository.GetPublicProfile(userId, "following_count,follower_count,follow_status,mark_status");
            if (user is null)
            {
                return OperationResult<UserProfile>.Fail("用户已注销");
            }
            user.Background = "assets/images/banner.jpg";
            return OperationResult.Ok(user);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IOperationResult<int> ToggleFollow(int user)
        {
            if (client.UserId == 0)
            {
                return OperationResult.Fail<int>("请先登录");
            }
            if (client.UserId == user || !db.Users.Where(i => i.Id == user).Any())
            {
                return OperationResult.Fail<int>("数据错误");
            }
            var relationship = new RelationshipRepository(db, client);
            var status = relationship.Toggle(user, RelationshipRepository.TYPE_FOLLOWING);
            if (status < 1)
            {
                return OperationResult.Ok(0);
            }
            return OperationResult.Ok(relationship.UserAlsoIs(client.UserId, user, RelationshipRepository.TYPE_FOLLOWING)
                ? 2 : 1);
        }

        /// <summary>
        /// 拉黑
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IOperationResult<int> ToggleMark(int user)
        {
            if (client.UserId == 0)
            {
                return OperationResult.Fail<int>("请先登录");
            }
            if (client.UserId == user || !db.Users.Where(i => i.Id == user).Any())
            {
                return OperationResult.Fail<int>("数据错误");
            }
            var status = new RelationshipRepository(db, client).Toggle(user, RelationshipRepository.TYPE_BLOCKING);
            return OperationResult.Ok(status < 1 ? 0 : 1);
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reason"></param>
        /// <exception cref="Exception"></exception>
        public IOperationResult Report(int userId, string reason)
        {
            if (client.UserId == 0)
            {
                return OperationResult.Fail("Please log in first");
            }
            if (client.UserId == userId)
            {
                return OperationResult.Fail("error");
            }
            var user = db.Users.Where(i => i.Id == userId).SingleOrDefault();
            if (user == null) 
            {
                return OperationResult.Fail("user is error");
            }
            mediator.Publish(ReportRequest.Create(client, ModuleTargetType.User,
                userId, "举报用户", $"举报【{user.Name}】：{reason}"));
            client.TryGetUser(out var currentUser);
            mediator.Publish(
                BulletinRequest.ToAdministrator(client, 
                "举报用户", 
                $"举报人：{currentUser?.Name}；被举报人：{user.Name}；举报理由：{reason}",
                BulletinType.Additional)
            );
            return OperationResult.Ok();
        }
    }
}
