using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FileQuoteEntity
    {
        internal const string ND_TABLE_NAME = "base_file_quote";
        public int Id { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
