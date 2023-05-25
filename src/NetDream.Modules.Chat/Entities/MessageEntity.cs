using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MessageEntity
    {
        internal const string ND_TABLE_NAME = "chat_message";
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("receive_id")]
        public int ReceiveId { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
