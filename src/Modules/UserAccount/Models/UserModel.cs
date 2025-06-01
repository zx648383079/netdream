using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserModel : UserProfileModel, IUserToken
    {

        public string Token { get; set; } = string.Empty;

        public UserModel()
        {
            
        }

        public UserModel(UserEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Avatar = entity.Avatar;
        }
    }
}
