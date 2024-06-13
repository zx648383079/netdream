using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderCouponEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_coupon";
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("coupon_id")]
        public int CouponId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
