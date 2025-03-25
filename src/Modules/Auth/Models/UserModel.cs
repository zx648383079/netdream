using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Models
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
