using NetDream.Core.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserEntity: IUser
    {
        internal const string ND_TABLE_NAME = "user";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public int Money { get; set; }
        public int Credits { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
