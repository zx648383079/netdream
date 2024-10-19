using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ListEntity
    {
        internal const string ND_TABLE_NAME = "book_list";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("book_count")]
        public int BookCount { get; set; }
        [Column("click_count")]
        public int ClickCount { get; set; }
        [Column("collect_count")]
        public int CollectCount { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
