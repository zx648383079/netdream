using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public ListLabelItem? Category { set; }
    }
}