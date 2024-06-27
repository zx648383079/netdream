using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace Modules.Forum.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ThreadEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "bbs_thread";
        public int Id { get; set; }
        [Column("forum_id")]
        public int ForumId { get; set; }
        [Column("classify_id")]
        public int ClassifyId { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("view_count")]
        public int ViewCount { get; set; }
        [Column("post_count")]
        public int PostCount { get; set; }
        [Column("collect_count")]
        public int CollectCount { get; set; }
        [Column("is_highlight")]
        public byte IsHighlight { get; set; }
        [Column("is_digest")]
        public byte IsDigest { get; set; }
        [Column("is_closed")]
        public byte IsClosed { get; set; }
        [Column("is_private_post")]
        public byte IsPrivatePost { get; set; }
        [Column("top_type")]
        public byte TopType { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
