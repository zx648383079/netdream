using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class DeliveryGoodsEntity
    {
        internal const string ND_TABLE_NAME = "shop_delivery_goods";
        public int Id { get; set; }
        [Column("delivery_id")]
        public int DeliveryId { get; set; }
        [Column("order_goods_id")]
        public int OrderGoodsId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        [Column("series_number")]
        public string SeriesNumber { get; set; } = string.Empty;
        public int Amount { get; set; }
        [Column("type_remark")]
        public string TypeRemark { get; set; } = string.Empty;
    }
}
