using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class HistoryEntity
    {
        internal const string ND_TABLE_NAME = "chat_history";
        public int Id { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("unread_count")]
        public int UnreadCount { get; set; }
        [Column("last_message")]
        public int LastMessage { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
