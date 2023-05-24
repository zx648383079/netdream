using NPoco;

namespace NetDream.Modules.Auth.Entities
{
    [TableName("user")]
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string Birthday { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public int Money { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
