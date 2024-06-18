using NetDream.Core.Interfaces.Entities;
using NetDream.Modules.Auth.Entities;
using NPoco;

namespace NetDream.Modules.Auth.Models
{
    [TableName(UserEntity.ND_TABLE_NAME)]
    public class UserSimpleModel : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;
    }
}
