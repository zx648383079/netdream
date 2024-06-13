using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CommentEntity
    {
        internal const string ND_TABLE_NAME = "blog_comment";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Position { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string Agent { get; set; } = string.Empty;
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        public int Approved { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
