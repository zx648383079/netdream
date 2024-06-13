using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FileLogEntity
    {
        internal const string ND_TABLE_NAME = "base_file_log";
        public int Id { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public string Data { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
