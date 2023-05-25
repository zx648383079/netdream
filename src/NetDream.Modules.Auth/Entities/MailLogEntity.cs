using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MailLogEntity
    {
        internal const string ND_TABLE_NAME = "user_mail_log";
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public int Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Amount { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
