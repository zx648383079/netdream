using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class VisitorLogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_visitor_log";
        public int Id { get; set; }
        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        [Column("first_at")]
        public int FirstAt { get; set; }
        [Column("last_at")]
        public int LastAt { get; set; }
    }
}
