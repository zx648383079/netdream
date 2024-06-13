using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AffiliateLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_affiliate_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("order_sn")]
        public string OrderSn { get; set; } = string.Empty;
        [Column("order_amount")]
        public decimal OrderAmount { get; set; }
        public decimal Money { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
