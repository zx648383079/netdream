using NetDream.Modules.TradeTracker.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Repositories
{
    public class ProductModel : ProductEntity
    {

        public ListLabelItem[] Items { get; set; }

        public ChannelEntity[] ChannelItems { get; set; }
    }
}
