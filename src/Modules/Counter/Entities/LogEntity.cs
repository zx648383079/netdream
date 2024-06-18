using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_log";
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        [Column("browser_version")]
        public string BrowserVersion { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        [Column("os_version")]
        public string OsVersion { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Referrer { get; set; } = string.Empty;
        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("session_id")]
        public string SessionId { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
