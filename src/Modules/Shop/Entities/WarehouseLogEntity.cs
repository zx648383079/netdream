using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class WarehouseLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_warehouse_log";
        public int Id { get; set; }
        [Column("warehouse_id")]
        public int WarehouseId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public int Amount { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        public string Remark { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
