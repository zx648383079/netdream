using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class WarehouseEntity
    {
        internal const string ND_TABLE_NAME = "shop_warehouse";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        [Column("link_user")]
        public string LinkUser { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
