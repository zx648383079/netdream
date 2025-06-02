using NetDream.Modules.UserAccount.Entities;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserModel : UserProfileModel
    {

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
