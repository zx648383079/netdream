using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ReplyEntity
    {
        internal const string ND_TABLE_NAME = "bot_reply";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        public string Event { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public byte Match { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        public byte Status { get; set; }
    }
}
