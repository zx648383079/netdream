namespace NetDream.Modules.Shop.Market
{
    public interface IProductSource
    {
        public string Name { get; }
        public decimal Price { get; }
        public decimal MarketPrice { get; }
        public decimal Weight { get; }
    }
}
