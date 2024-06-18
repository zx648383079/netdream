using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MediaEntity
    {
        internal const string ND_TABLE_NAME = "bot_media";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        public string Type { get; set; } = string.Empty;
        [Column("material_type")]
        public byte MaterialType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        [Column("show_cover")]
        public byte ShowCover { get; set; }
        [Column("open_comment")]
        public byte OpenComment { get; set; }
        [Column("only_comment")]
        public byte OnlyComment { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("media_id")]
        public string MediaId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [Column("publish_status")]
        public byte PublishStatus { get; set; }
        [Column("publish_id")]
        public string PublishId { get; set; } = string.Empty;
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
