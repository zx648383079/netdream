using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.OpenPlatform.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PlatformEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "open_platform";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string Appid { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        [Column("sign_type")]
        public byte SignType { get; set; }
        [Column("sign_key")]
        public string SignKey { get; set; } = string.Empty;
        [Column("encrypt_type")]
        public byte EncryptType { get; set; }
        [Column("public_key")]
        public string PublicKey { get; set; } = string.Empty;
        public string Rules { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("allow_self")]
        public int AllowSelf { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
