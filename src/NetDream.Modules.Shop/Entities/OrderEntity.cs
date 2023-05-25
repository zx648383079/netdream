using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderEntity
    {
        internal const string ND_TABLE_NAME = "shop_order";
        public int Id { get; set; }
        [Column("series_number")]
        public string SeriesNumber { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [Column("payment_name")]
        public string PaymentName { get; set; } = string.Empty;
        [Column("shipping_id")]
        public int ShippingId { get; set; }
        [Column("invoice_id")]
        public int InvoiceId { get; set; }
        [Column("shipping_name")]
        public string ShippingName { get; set; } = string.Empty;
        [Column("goods_amount")]
        public decimal GoodsAmount { get; set; }
        [Column("order_amount")]
        public decimal OrderAmount { get; set; }
        public decimal Discount { get; set; }
        [Column("shipping_fee")]
        public decimal ShippingFee { get; set; }
        [Column("pay_fee")]
        public decimal PayFee { get; set; }
        [Column("pay_at")]
        public int PayAt { get; set; }
        [Column("shipping_at")]
        public int ShippingAt { get; set; }
        [Column("receive_at")]
        public int ReceiveAt { get; set; }
        [Column("finish_at")]
        public int FinishAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("reference_type")]
        public int ReferenceType { get; set; }
        [Column("reference_id")]
        public int ReferenceId { get; set; }
    }
}
