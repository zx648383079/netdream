using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Market.Models
{
    public class ProductVerifyResult
    {
        public GoodsEntity? Goods { get; set; }

        public ProductEntity? Product { get; set; }

        public bool PropertyIsValid { get; set; }
    }
}
