using NPoco;

namespace NetDream.Razor.Entities
{
    [TableName("blog_term")]
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Thumb { get; set; }
        public string Styles { get; set; }

        [Ignore]
        public int BlogCount { get; set; } = 0;
    }
}
