using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class RegionEntity
    {
        internal const string ND_TABLE_NAME = "shop_region";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;
    }
}
