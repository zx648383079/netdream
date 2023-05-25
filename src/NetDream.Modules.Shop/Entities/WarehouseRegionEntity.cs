using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class WarehouseRegionEntity
    {
        internal const string ND_TABLE_NAME = "shop_warehouse_region";
        [Column("warehouse_id")]
        public int WarehouseId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
    }
}
