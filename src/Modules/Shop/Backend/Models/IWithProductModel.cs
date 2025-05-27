namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithProductModel
    {
        public int ProductId { get; }
        public ProductLabelItem? Product { set; }
    }
}