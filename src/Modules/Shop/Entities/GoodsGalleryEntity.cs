using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsGalleryEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods_gallery";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public string File { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
