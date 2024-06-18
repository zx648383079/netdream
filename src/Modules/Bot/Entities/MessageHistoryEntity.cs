using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MessageHistoryEntity
    {
        internal const string ND_TABLE_NAME = "bot_message_history";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        public byte Type { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("is_mark")]
        public byte IsMark { get; set; }
    }
}
