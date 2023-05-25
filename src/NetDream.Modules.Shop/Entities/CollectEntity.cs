using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CollectEntity
    {
        internal const string ND_TABLE_NAME = "shop_collect";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
