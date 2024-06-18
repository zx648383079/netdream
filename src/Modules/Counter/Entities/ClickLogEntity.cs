using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ClickLogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_click_log";
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        [Column("session_id")]
        public string SessionId { get; set; } = string.Empty;
        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        [Column("tag_url")]
        public string TagUrl { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
