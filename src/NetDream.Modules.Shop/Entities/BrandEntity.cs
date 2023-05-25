using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BrandEntity
    {
        internal const string ND_TABLE_NAME = "shop_brand";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        [Column("app_logo")]
        public string AppLogo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
