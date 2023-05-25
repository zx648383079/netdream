using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LoginLogEntity
    {
        internal const string ND_TABLE_NAME = "user_login_log";
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public string User { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Mode { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
    }
}
