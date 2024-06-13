using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class MetaEntity
    {
        internal const string ND_TABLE_NAME = "blog_meta";
        public int Id { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
