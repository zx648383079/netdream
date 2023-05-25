using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ShippingRegionEntity
    {
        internal const string ND_TABLE_NAME = "shop_shipping_region";
        [Column("shipping_id")]
        public int ShippingId { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
    }
}
