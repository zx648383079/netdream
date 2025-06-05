using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class IssueListItem : GoodsIssueEntity, IWithGoodsModel
    {
        public GoodsLabelItem? Goods { get; set; }
    }
}
