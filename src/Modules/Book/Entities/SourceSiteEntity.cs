using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookSourceSiteEntity
    {
        internal const string ND_TABLE_NAME = "book_source_site";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
