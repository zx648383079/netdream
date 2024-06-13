using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class WarehouseGoodsEntity
    {
        internal const string ND_TABLE_NAME = "shop_warehouse_goods";
        public int Id { get; set; }
        [Column("warehouse_id")]
        public int WarehouseId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
