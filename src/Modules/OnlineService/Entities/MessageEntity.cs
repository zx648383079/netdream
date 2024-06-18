using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MessageEntity
    {
        internal const string ND_TABLE_NAME = "service_message";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("session_id")]
        public int SessionId { get; set; }
        [Column("send_type")]
        public byte SendType { get; set; }
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
