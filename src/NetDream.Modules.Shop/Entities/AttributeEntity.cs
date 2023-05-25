using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AttributeEntity
    {
        internal const string ND_TABLE_NAME = "shop_attribute";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("group_id")]
        public int GroupId { get; set; }
        public int Type { get; set; }
        [Column("search_type")]
        public int SearchType { get; set; }
        [Column("input_type")]
        public int InputType { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
