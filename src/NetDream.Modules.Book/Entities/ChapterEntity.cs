using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ChapterEntity
    {
        internal const string ND_TABLE_NAME = "book_chapter";
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
        public int Position { get; set; }
        public int Size { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
