using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MenuEntity
    {
        internal const string ND_TABLE_NAME = "bot_menu";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
