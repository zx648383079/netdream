using NPoco;
namespace Modules.MicroBlog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlogEntity
    {
        internal const string ND_TABLE_NAME = "micro_blog";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;
        [Column("open_type")]
        public byte OpenType { get; set; }
        [Column("recommend_count")]
        public int RecommendCount { get; set; }
        [Column("collect_count")]
        public int CollectCount { get; set; }
        [Column("forward_count")]
        public int ForwardCount { get; set; }
        [Column("comment_count")]
        public int CommentCount { get; set; }
        [Column("forward_id")]
        public int ForwardId { get; set; }
        public string Source { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
