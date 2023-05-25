using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FriendClassifyEntity
    {
        internal const string ND_TABLE_NAME = "chat_friend_classify";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
