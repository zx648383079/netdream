using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsAttributeEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods_attribute";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("attribute_id")]
        public int AttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
