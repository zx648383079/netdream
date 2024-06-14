using NPoco;
namespace Modules.MicroBlog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TopicEntity
    {
        internal const string ND_TABLE_NAME = "micro_topic";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
