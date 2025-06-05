namespace NetDream.Modules.Shop.Market.Models
{
    public interface ICartItem
    {
        public int Amount { get; }

        public float Price { get; }

        public float Total { get; }

        public IGoodsSource Goods { get; }
    }

    public interface IGoodsSource
    {
        public string Name { get; }
        public float Price { get; }
        public float MarketPrice { get; }
        public float Weight { get; }
    }
}
