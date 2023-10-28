using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Position { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("model_id")]
        public int ModelId { get; set; }
        [Column("content_id")]
        public int ContentId { get; set; }
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
