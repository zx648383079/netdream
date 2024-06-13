using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class InviteCodeEntity
    {
        internal const string ND_TABLE_NAME = "user_invite_code";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Amount { get; set; }
        [Column("invite_count")]
        public int InviteCount { get; set; }
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
