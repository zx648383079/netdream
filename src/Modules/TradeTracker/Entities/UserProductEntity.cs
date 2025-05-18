using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.TradeTracker.Entities
{
    public class UserProductEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public decimal Price { get; set; }
        public decimal SellPrice { get; set; }
        public int SellChannelId { get; set; }
        public int Status { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
