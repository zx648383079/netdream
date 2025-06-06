using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Market.Models
{
    public class ProductPropertyResult
    {
        public GoodsProperty[] Properties { get; internal set; } = [];
        public GoodsPropertyCollection[] StaticProperties { get; internal set; } = [];
    }
}
