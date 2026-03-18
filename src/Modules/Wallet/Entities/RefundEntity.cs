using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Wallet.Entities
{
    public class RefundEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int TradeId { get; internal set; }
        public string OutRequestNo { get; internal set; }
        public string RefundReason { get; internal set; }
        public float RefundAmount { get; internal set; }

        public string OperatorId { get; internal set; }
        public byte Status { get; internal set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }

    }
}
