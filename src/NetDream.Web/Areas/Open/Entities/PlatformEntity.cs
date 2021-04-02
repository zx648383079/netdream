using NPoco;

namespace NetDream.Web.Areas.Open.Entities
{
    [TableName("open_platform")]
    public class PlatformEntity
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public string Appid { get; set; }
        public string Secret { get; set; }
        [Column("sign_type")]
        public int SignType { get; set; }
        [Column("sign_key")]
        public string SignKey { get; set; }
        [Column("encrypt_type")]
        public int EncryptType { get; set; }
        [Column("public_key")]
        public string PublicKey { get; set; }
        public string Rules { get; set; }
        [Column("allow_self")]
        public int AllowSelf { get; set; }
        public int Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
