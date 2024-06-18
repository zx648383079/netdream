using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BotEntity
    {
        internal const string ND_TABLE_NAME = "bot";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        [Column("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Original { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Column("platform_type")]
        public byte PlatformType { get; set; }
        public string Appid { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        [Column("aes_key")]
        public string AesKey { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Qrcode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
