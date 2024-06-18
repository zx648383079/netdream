using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class JumpLogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_jump_log";
        public int Id { get; set; }
        public string Referrer { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        [Column("session_id")]
        public string SessionId { get; set; } = string.Empty;
        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
