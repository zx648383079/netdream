using NetDream.Modules.TradeTracker.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Models
{
    public class ProductListItem : ProductEntity, IWithCategoryModel, IWithProjectModel
    {
        public ListLabelItem? Project {  get; set; }
        public ListLabelItem? Category { get; set; }
    }
}
