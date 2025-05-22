namespace NetDream.Modules.Trade.Entities
{
    public class TradeEntity
    {
        public uint Id { get; internal set; }
        public uint OpenId { get; internal set; }
        public string BuyerId { get; internal set; }
        public string SellerId { get; internal set; }
        public string OutTradeNo { get; internal set; }
        public string Subject { get; internal set; }
        public string Body { get; internal set; }
        public float TotalAmount { get; internal set; }
        public uint TimeExpire { get; internal set; }
        public string NotifyUrl { get; internal set; }
        public string OperatorId { get; internal set; }
        public string ReturnUrl { get; internal set; }
        public string PassbackParams { get; internal set; }
        public byte Status { get; internal set; }
        public uint UpdatedAt { get; internal set; }
        public uint CreatedAt { get; internal set; }
    }
}
