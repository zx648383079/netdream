using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OauthEntity
    {
        internal const string ND_TABLE_NAME = "user_oauth";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public string Unionid { get; set; } = string.Empty;
        public string Identity { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
