using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Market.Models
{
    public class ShippingListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Method { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ShippingGroupEntity? Settings { get; internal set; }
        public float ShippingFee { get; internal set; }
    }
}
