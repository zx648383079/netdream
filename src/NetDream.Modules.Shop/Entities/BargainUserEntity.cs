using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BargainUserEntity
    {
        internal const string ND_TABLE_NAME = "shop_bargain_user";
        public int Id { get; set; }
        [Column("act_id")]
        public int ActId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
