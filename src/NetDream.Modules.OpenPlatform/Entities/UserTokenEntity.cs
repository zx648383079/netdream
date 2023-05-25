using NPoco;
namespace NetDream.Modules.OpenPlatform.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserTokenEntity
    {
        internal const string ND_TABLE_NAME = "open_user_token";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        public string Token { get; set; } = string.Empty;
        [Column("is_self")]
        public int IsSelf { get; set; }
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
