using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.MicroBlog.Models
{
    public class UserOpenResult : IUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }
        public int MicroCount { get; set; }
        public int FollowingCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowStatus { get; set; }

        public UserOpenResult(IUser data)
        {
            Id = data.Id;
            Name = data.Name;
            Avatar = data.Avatar;
        }
    }
}
