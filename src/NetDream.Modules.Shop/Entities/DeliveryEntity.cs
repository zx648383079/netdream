using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class DeliveryEntity
    {
        internal const string ND_TABLE_NAME = "shop_delivery";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        public int Status { get; set; }
        [Column("shipping_id")]
        public int ShippingId { get; set; }
        [Column("shipping_name")]
        public string ShippingName { get; set; } = string.Empty;
        [Column("shipping_fee")]
        public decimal ShippingFee { get; set; }
        [Column("logistics_number")]
        public string LogisticsNumber { get; set; } = string.Empty;
        [Column("logistics_content")]
        public string LogisticsContent { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("region_name")]
        public string RegionName { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Column("best_time")]
        public string BestTime { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
