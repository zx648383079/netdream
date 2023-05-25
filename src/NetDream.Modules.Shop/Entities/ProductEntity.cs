using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ProductEntity
    {
        internal const string ND_TABLE_NAME = "shop_product";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        [Column("market_price")]
        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        public float Weight { get; set; }
        [Column("series_number")]
        public string SeriesNumber { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
