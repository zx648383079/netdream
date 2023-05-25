using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderActivityEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_activity";
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
