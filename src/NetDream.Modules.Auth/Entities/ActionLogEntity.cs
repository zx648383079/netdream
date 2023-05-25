using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ActionLogEntity
    {
        internal const string ND_TABLE_NAME = "user_action_log";
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
