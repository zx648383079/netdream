using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class StayTimeLogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_stay_time_log";
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;
        [Column("session_id")]
        public string SessionId { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("enter_at")]
        public int EnterAt { get; set; }
        [Column("leave_at")]
        public int LeaveAt { get; set; }
    }
}
