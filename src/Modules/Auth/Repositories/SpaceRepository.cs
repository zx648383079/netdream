using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using NPoco;
using System;

namespace NetDream.Modules.Auth.Repositories
{
    public class SpaceRepository(IDatabase db, 
        IClientContext client, 
        UserRepository repository,
        RelationshipRepository relationship,
        ISystemFeedback feedback,
        ISystemBulletin bulletin)
    {
        public UserProfile Get(int userId)
        {
            var user = repository.GetPublicProfile(userId, "following_count,follower_count,follow_status,mark_status");
            if (user is null)
            {
                throw new Exception("用户已注销");
            }
            user.Background = "assets/images/banner.jpg";
            return user;
        }

        /**
         * 关注
         * @param int user
         * @return int
         * @throws Exception
         */
        public int ToggleFollow(int user)
        {
            if (client.UserId == 0)
            {
                return 0;
            }
            var status = relationship.Toggle(user, RelationshipRepository.TYPE_FOLLOWING);
            if (status < 1)
            {
                return 0;
            }
            return relationship.UserAlsoIs(client.UserId, user, RelationshipRepository.TYPE_FOLLOWING)
                ? 2 : 1;
        }

        /**
         * 拉黑
         * @param int user
         * @return int
         * @throws Exception
         */
        public int ToggleMark(int user)
        {
            if (client.UserId == 0)
            {
                return 0;
            }
            var status = relationship.Toggle(user, RelationshipRepository.TYPE_BLOCKING);
            return status < 1 ? 0 : 1;
        }

        /**
         * 举报
         * @param int userId
         * @return void
         * @throws Exception
         */
        public void Report(int userId, string reason)
        {
            if (client.UserId == 0)
            {
                throw new Exception("Please log in first");
            }
            if (client.UserId == userId)
            {
                throw new Exception("error");
            }
            var user = db.SingleById<UserEntity>(userId) ?? throw new Exception("user is error");
            feedback.Report(ModuleModelType.TYPE_USER,
                userId, $"举报【{user.Name}】：{reason}", "举报用户");
            client.TryGetUser(out var currentUser);
            bulletin.SendAdministrator("举报用户",
                $"举报人：{currentUser?.Name}；被举报人：{user.Name}；举报理由：{reason}", 98
            );
        }
    }
}
