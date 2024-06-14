using NPoco;
namespace Modules.Forum.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ThreadPostEntity
    {
        internal const string ND_TABLE_NAME = "bbs_thread_post";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("thread_id")]
        public int ThreadId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Ip { get; set; } = string.Empty;
        public int Grade { get; set; }
        [Column("is_invisible")]
        public byte IsInvisible { get; set; }
        public byte Status { get; set; }
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
