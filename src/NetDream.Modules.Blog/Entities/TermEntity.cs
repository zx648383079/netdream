using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TermEntity
    {
        internal const string ND_TABLE_NAME = "blog_term";
        public int Id { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Styles { get; set; } = string.Empty;
        [Column("en_name")]
        public string EnName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
