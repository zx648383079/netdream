using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsCardEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods_card";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("card_no")]
        public string CardNo { get; set; } = string.Empty;
        [Column("card_pwd")]
        public string CardPwd { get; set; } = string.Empty;
        [Column("order_id")]
        public int OrderId { get; set; }
    }
}
