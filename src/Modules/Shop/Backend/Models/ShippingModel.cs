using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class ShippingModel : ShippingEntity
    {
        public ShippingGroupModel[] Groups { get; internal set; }
    }
}
