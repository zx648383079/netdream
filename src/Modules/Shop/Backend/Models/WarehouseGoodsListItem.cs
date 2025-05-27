using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class WarehouseGoodsListItem : WarehouseGoodsEntity, IWithWarehouseModel, IWithGoodsModel, IWithProductModel
    {
        public ListLabelItem? Warehouse { get; set; }
        public GoodsLabelItem? Goods { get; set; }
        public ProductLabelItem? Product { get; set; }
    }
}
