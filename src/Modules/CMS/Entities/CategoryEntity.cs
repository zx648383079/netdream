
using NPoco;

namespace NetDream.Modules.CMS.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Column("model_id")]
        public int ModelId { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public byte Position { get; set; }
        public string Groups { get; set; } = string.Empty;
        [Column("category_template")]
        public string CategoryTemplate { get; set; } = string.Empty;
        [Column("list_template")]
        public string ListTemplate { get; set; } = string.Empty;
        [Column("show_template")]
        public string ShowTemplate { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
