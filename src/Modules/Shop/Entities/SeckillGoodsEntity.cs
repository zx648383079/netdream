using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SeckillGoodsEntity
    {
        internal const string ND_TABLE_NAME = "shop_seckill_goods";
        public int Id { get; set; }
        [Column("act_id")]
        public int ActId { get; set; }
        [Column("time_id")]
        public int TimeId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        [Column("every_amount")]
        public int EveryAmount { get; set; }
    }
}
