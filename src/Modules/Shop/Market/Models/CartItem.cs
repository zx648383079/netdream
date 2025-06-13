namespace NetDream.Modules.Shop.Market.Models
{
    public class CartItem : ICartItem
    {
        public int Id { get; set; }

        public int SelectedActivity { get; set; }

        public bool IsChecked { get; set; }

        public int ExpiredAt { get; set; }
        public int Amount {  get; set; }

        public decimal Price { get; set; }

        public decimal Total => Amount * Price;

        public int GoodsId { get; internal set; }
        public int ProductId { get; internal set; }
        public string AttributeId { get; internal set; }
        public string AttributeValue { get; internal set; }

        public IProductSource Goods { get; set; }


    }
}
