using NPoco;
namespace NetDream.Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ShareEntity
    {
        internal const string ND_TABLE_NAME = "disk_share";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Mode { get; set; }
        public string Password { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("death_at")]
        public int DeathAt { get; set; }
        [Column("view_count")]
        public int ViewCount { get; set; }
        [Column("down_count")]
        public int DownCount { get; set; }
        [Column("save_count")]
        public int SaveCount { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
