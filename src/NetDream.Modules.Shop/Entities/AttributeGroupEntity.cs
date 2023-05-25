using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AttributeGroupEntity
    {
        internal const string ND_TABLE_NAME = "shop_attribute_group";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
