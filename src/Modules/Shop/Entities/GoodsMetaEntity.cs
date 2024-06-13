using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsMetaEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods_meta";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
