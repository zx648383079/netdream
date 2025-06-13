using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Market.Models
{
    public class ShippingListItem : ICodeOptionItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Method { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool CodEnabled { get; set; }
        public ShippingGroupEntity? Settings { get; internal set; }
        public decimal ShippingFee { get; internal set; }
    }
}
