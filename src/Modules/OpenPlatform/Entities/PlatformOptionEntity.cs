using NPoco;
namespace NetDream.Modules.OpenPlatform.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PlatformOptionEntity
    {
        internal const string ND_TABLE_NAME = "open_platform_option";
        public int Id { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        public string Store { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
