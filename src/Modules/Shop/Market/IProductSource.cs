namespace NetDream.Modules.Shop.Market
{
    public interface IProductSource
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; }
        public decimal Price { get; }
        public decimal MarketPrice { get; }
        public decimal Weight { get; }
    }

    public interface IProductHost
    {
        public IProductSource Goods { get; }
    }
}
