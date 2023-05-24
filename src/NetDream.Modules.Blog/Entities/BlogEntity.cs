using NPoco;

namespace NetDream.Modules.Blog.Entities
{
    [TableName("blog")]
    public class BlogEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("programming_language")]
        public string ProgrammingLanguage { get; set; }
        public string Language { get; set; }
        public string Thumb { get; set; }
        [Column("edit_type")]
        public int EditType { get; set; }
        public string Content { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("term_id")]
        public int TermId { get; set; }
        public int Type { get; set; }
        [Column("recommend_count")]
        public int RecommendCount { get; set; }
        [Column("comment_count")]
        public int CommentCount { get; set; }
        [Column("click_count")]
        public int ClickCount { get; set; }
        [Column("open_type")]
        public int OpenType { get; set; }
        [Column("open_rule")]
        public string OpenRule { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
