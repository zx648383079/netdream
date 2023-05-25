using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BanAccountEntity
    {
        internal const string ND_TABLE_NAME = "user_ban_account";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_key")]
        public string ItemKey { get; set; } = string.Empty;
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
