using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class UserProfileModel : UserListItem, IUserProfile
    {
        public int BulletinCount { get; set; }
    }
}
