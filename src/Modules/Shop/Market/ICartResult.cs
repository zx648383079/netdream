using System.Collections.Generic;

namespace NetDream.Modules.Shop.Market
{
    public interface ICartResult
    {
        public IList<ICartGroupItem> Data { get; }

        public ICheckoutButton CheckoutButton {get;}

        public ICartCell[] PromotionCell { get;}

        public ICartSubtotal Subtotal { get; }
    }

    public interface ICheckoutButton
    {
        public string Text { get; }
        public string Action { get; }
    }

    public interface ICartCell
    {
        public string PopupTip { get; }
        public ICartLink? Link { get; }
    }

    public interface ICartLink
    {
        public string Text { get; }
        public string Url { get; }
    }

    public interface ICartSubtotal
    {
        public decimal Total { get; }
        public decimal TotalWeight { get; }
        public decimal OriginalTotal { get; }
        public decimal DiscountAmount { get; }
        public int Count { get; }
    }
}
