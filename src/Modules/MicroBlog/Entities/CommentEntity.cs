using NPoco;
namespace Modules.MicroBlog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CommentEntity
    {
        internal const string ND_TABLE_NAME = "micro_comment";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("target_id")]
        public int TargetId { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
