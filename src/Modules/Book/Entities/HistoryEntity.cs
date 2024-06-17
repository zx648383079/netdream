using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookHistoryEntity
    {
        internal const string ND_TABLE_NAME = "book_history";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        [Column("chapter_id")]
        public int ChapterId { get; set; }
        public int Progress { get; set; }
        [Column("source_id")]
        public int SourceId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
