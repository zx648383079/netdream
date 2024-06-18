using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class QrcodeEntity
    {
        internal const string ND_TABLE_NAME = "bot_qrcode";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        public byte Type { get; set; }
        [Column("scene_type")]
        public byte SceneType { get; set; }
        [Column("scene_str")]
        public string SceneStr { get; set; } = string.Empty;
        [Column("scene_id")]
        public int SceneId { get; set; }
        [Column("expire_time")]
        public int ExpireTime { get; set; }
        [Column("qr_url")]
        public string QrUrl { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
