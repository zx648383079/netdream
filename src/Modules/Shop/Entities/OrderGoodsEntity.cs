using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderGoodsEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_goods";
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("series_number")]
        public string SeriesNumber { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        [Column("refund_id")]
        public int RefundId { get; set; }
        public int Status { get; set; }
        [Column("after_sale_status")]
        public int AfterSaleStatus { get; set; }
        [Column("comment_id")]
        public int CommentId { get; set; }
        [Column("type_remark")]
        public string TypeRemark { get; set; } = string.Empty;
    }
}
