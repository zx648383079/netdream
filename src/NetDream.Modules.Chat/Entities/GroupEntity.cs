using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GroupEntity
    {
        internal const string ND_TABLE_NAME = "chat_group";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
