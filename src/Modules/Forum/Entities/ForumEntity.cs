using NPoco;
namespace Modules.Forum.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ForumEntity
    {
        internal const string ND_TABLE_NAME = "bbs_forum";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("thread_count")]
        public int ThreadCount { get; set; }
        [Column("post_count")]
        public int PostCount { get; set; }
        public byte Type { get; set; }
        public byte Position { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
