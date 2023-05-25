using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryEntity
    {
        internal const string ND_TABLE_NAME = "shop_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Banner { get; set; } = string.Empty;
        [Column("app_banner")]
        public string AppBanner { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Position { get; set; }
    }
}
