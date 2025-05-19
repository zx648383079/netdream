using NetDream.Modules.TradeTracker.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Models
{
    public class LogListItem : IWithChannelModel, IWithProductModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public byte Type { get; set; }
        public float Price { get; set; }
        public int CreatedAt { get; set; }

        public ChannelEntity? Channel { get; set; }
        public ListLabelItem? Product { get; set; }

    }
}
