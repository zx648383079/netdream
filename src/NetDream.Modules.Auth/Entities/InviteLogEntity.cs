using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class InviteLogEntity
    {
        internal const string ND_TABLE_NAME = "user_invite_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Code { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
