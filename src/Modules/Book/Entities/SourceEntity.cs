using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookSourceEntity
    {
        internal const string ND_TABLE_NAME = "book_source";
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        [Column("size_id")]
        public int SizeId { get; set; }
        public string Url { get; set; } = string.Empty;
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
