using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.TradeTracker.Entities
{
    public class ChannelEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public string ShortName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SiteUrl { get; set; } = string.Empty;
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
