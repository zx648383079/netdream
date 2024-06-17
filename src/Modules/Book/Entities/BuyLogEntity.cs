using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookBuyLogEntity
    {
        internal const string ND_TABLE_NAME = "book_buy_log";
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        [Column("chapter_id")]
        public int ChapterId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
