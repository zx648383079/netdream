using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TagEntity
    {
        internal const string ND_TABLE_NAME = "blog_tag";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("blog_count")]
        public int BlogCount { get; set; }
    }
}
