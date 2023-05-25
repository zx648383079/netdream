using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookEntity
    {
        internal const string ND_TABLE_NAME = "book";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("author_id")]
        public int AuthorId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Classify { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        public int Size { get; set; }
        [Column("click_count")]
        public int ClickCount { get; set; }
        [Column("recommend_count")]
        public int RecommendCount { get; set; }
        [Column("over_at")]
        public int OverAt { get; set; }
        public int Status { get; set; }
        [Column("source_type")]
        public int SourceType { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
