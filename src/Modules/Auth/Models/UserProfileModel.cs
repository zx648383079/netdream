using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Auth.Models
{
    [TableName(UserEntity.ND_TABLE_NAME)]
    public class UserProfileModel : UserSimpleModel, IUserProfile
    {
        public int BulletinCount { get; set; }
    }
}
