using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.TradeTracker.Entities
{
    public class ChannelProductEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public string PlatformNo { get; set; } = string.Empty;
        public string ExtraMeta { get; set; } = string.Empty;
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
