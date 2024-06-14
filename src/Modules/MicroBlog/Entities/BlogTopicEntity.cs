using NPoco;
namespace Modules.MicroBlog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlogTopicEntity
    {
        internal const string ND_TABLE_NAME = "micro_blog_topic";
        public int Id { get; set; }
        [Column("micro_id")]
        public int MicroId { get; set; }
        [Column("topic_id")]
        public int TopicId { get; set; }
    }
}
