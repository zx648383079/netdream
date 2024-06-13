using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderRefundEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_refund";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("order_goods_id")]
        public int OrderGoodsId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Amount { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public decimal Money { get; set; }
        [Column("order_price")]
        public decimal OrderPrice { get; set; }
        public int Freight { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
