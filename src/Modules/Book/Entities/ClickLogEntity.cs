using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookClickLogEntity
    {
        internal const string ND_TABLE_NAME = "book_click_log";
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        public int Hits { get; set; }
        [Column("created_at")]
        public string CreatedAt { get; set; } = string.Empty;
    }
}
