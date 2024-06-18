using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SessionLogEntity
    {
        internal const string ND_TABLE_NAME = "service_session_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("session_id")]
        public int SessionId { get; set; }
        public string Remark { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
