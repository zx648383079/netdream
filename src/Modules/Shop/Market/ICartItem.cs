namespace NetDream.Modules.Shop.Market
{
    public interface ICartItem : IProductHost
    {
        public int Id { get; }
        public int GoodsId { get; }
        public int ProductId { get; }
        public int Amount { get; }

        public decimal Price { get; }

        public decimal Total { get; }

        public int SelectedActivity { get; }

        public bool IsChecked { get; }

        public int ExpiredAt { get; }

        public IProductSource Goods { get; }

    }
}
