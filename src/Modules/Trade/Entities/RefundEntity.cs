namespace NetDream.Modules.Trade.Entities
{
    public class RefundEntity
    {
        public uint Id { get; internal set; }
        public uint TradeId { get; internal set; }
        public string OutRequestNo { get; internal set; }
        public string RefundReason { get; internal set; }
        public float RefundAmount { get; internal set; }

        public string OperatorId { get; internal set; }
        public byte Status { get; internal set; }
        public uint UpdatedAt { get; internal set; }
        public uint CreatedAt { get; internal set; }

    }
}
