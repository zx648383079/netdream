using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.TradeTracker.Entities
{
    public class TradeLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public byte Type { get; set; }
        public float Price { get; set; }
        public int CreatedAt { get; set; }
    }
}
