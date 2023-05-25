using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FriendEntity
    {
        internal const string ND_TABLE_NAME = "chat_friend";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("classify_id")]
        public int ClassifyId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("belong_id")]
        public int BelongId { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
