using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Models
{
    internal interface IWithProductModel
    {
        public int ProductId { get; }

        public ListLabelItem? Product { set; }
    }
}
