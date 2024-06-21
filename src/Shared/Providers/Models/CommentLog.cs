using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Shared.Providers.Models
{
    public class CommentLog: IIdEntity, ICreatedEntity
    {
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
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }

        [Ignore]
        public byte AgreeType { get; set; }
    }
}
