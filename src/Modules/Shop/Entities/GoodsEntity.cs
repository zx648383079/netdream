using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods";
        public int Id { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        [Column("brand_id")]
        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("series_number")]
        public string SeriesNumber { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [Column("market_price")]
        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        [Column("attribute_group_id")]
        public int AttributeGroupId { get; set; }
        public float Weight { get; set; }
        [Column("shipping_id")]
        public int ShippingId { get; set; }
        public int Sales { get; set; }
        [Column("is_best")]
        public int IsBest { get; set; }
        [Column("is_hot")]
        public int IsHot { get; set; }
        [Column("is_new")]
        public int IsNew { get; set; }
        public int Status { get; set; }
        [Column("admin_note")]
        public string AdminNote { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Position { get; set; }
        [Column("dynamic_position")]
        public int DynamicPosition { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("cost_price")]
        public decimal CostPrice { get; set; }
    }
}
