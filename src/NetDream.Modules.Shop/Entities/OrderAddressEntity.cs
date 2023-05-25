using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderAddressEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_address";
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("region_name")]
        public string RegionName { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Column("best_time")]
        public string BestTime { get; set; } = string.Empty;
    }
}
