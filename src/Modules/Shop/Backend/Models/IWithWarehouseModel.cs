using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithWarehouseModel
    {
        public int WarehouseId { get; }
        public ListLabelItem? Warehouse { set; }
    }
}