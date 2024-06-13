using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PayLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_pay_log";
        public int Id { get; set; }
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [Column("payment_name")]
        public string PaymentName { get; set; } = string.Empty;
        public int Type { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Data { get; set; } = string.Empty;
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        [Column("currency_money")]
        public decimal CurrencyMoney { get; set; }
        [Column("trade_no")]
        public string TradeNo { get; set; } = string.Empty;
        [Column("begin_at")]
        public int BeginAt { get; set; }
        [Column("confirm_at")]
        public int ConfirmAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
