using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Models
{
    internal interface IWithChannelModel
    {
        public int ChannelId { get; }

        public ChannelEntity? Channel { set; }
    }
}
