using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithBrandModel
    {
        public int BrandId { get; }

        public ListLabelItem? Brand { set; }
    }
}