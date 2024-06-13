using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CouponLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_coupon_log";
        public int Id { get; set; }
        [Column("coupon_id")]
        public int CouponId { get; set; }
        [Column("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("used_at")]
        public int UsedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
