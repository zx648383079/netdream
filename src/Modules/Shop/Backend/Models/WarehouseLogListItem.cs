using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class WarehouseLogListItem : WarehouseLogEntity, IWithWarehouseModel, IWithGoodsModel, IWithUserModel
    {
        public ListLabelItem? Warehouse { get; set; }
        public GoodsLabelItem? Goods { get; set; }
        public IUser? User { get; set; }
    }
}
