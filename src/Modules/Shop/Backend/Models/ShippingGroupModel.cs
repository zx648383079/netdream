using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class ShippingGroupModel : ShippingGroupEntity
    {
        public ListLabelItem[]? Regions { get; set; }
    }
}
