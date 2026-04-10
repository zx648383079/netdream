using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithBrandModel
    {
        public int BrandId { get; }

        public IListLabelItem? Brand { set; }
    }
}