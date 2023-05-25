using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LoginQrEntity
    {
        internal const string ND_TABLE_NAME = "user_login_qr";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
    }
}
