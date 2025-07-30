using MediatR;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class UserRepository(MicroBlogContext db,
        IUserRepository userStore,
        IMediator mediator)
    {

        public IOperationResult<UserOpenResult> Get(int user)
        {
            var model = userStore.Get(user);
            if (model is null)
            {
                return OperationResult<UserOpenResult>.Fail("用户已注销");
            }
            var res = new UserOpenResult(model);
            var data = new UserOpenStatisticsRequest(user, 
                ["following_count", 
                "follower_count", 
                "follow_status"]);
            mediator.Publish(data).GetAwaiter().GetResult();
            res.FollowerCount = (int)data.Result["follower_count"];
            res.FollowingCount = (int)data.Result["following_count"];
            res.FollowStatus = (int)data.Result["follow_status"];
            return OperationResult.Ok(res);
        }
    }
}
