using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TagRelationshipEntity
    {
        internal const string ND_TABLE_NAME = "blog_tag_relationship";
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public int Position { get; set; }
    }
}
