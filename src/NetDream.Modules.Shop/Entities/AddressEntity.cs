using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AddressEntity
    {
        internal const string ND_TABLE_NAME = "shop_address";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
