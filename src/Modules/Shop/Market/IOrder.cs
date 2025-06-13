using System.Collections.Generic;

namespace NetDream.Modules.Shop.Market
{
    public interface IOrder
    {
        public IList<IOrderProduct> Goods { get; }
        public IOrderAddress? Address { get; set; }
    }

    public interface IOrderProduct
    {
        public int GoodsId { get; }

        public int ProductId { get; }
        public string Name { get; }

        public string SeriesNumber { get; }
        public string Thumb { get; }
        public int Amount { get; }
        public decimal Price { get; }
        public decimal Discount { get; }
    }

    public interface IOrderAddress
    {
        public string Name { get; }

        public int RegionId { get; }

        public string RegionName { get; }
        public string Tel { get; }
        public string Address { get; }

        public string BestTime { get; }
    }

    public interface IOrderCoupon
    {
        public int CouponId { get; }
        public string Name { get; }
        public string Type { get; }
    }

    public interface IOrderActivity
    {
        public int ProductId { get; }
        public byte Type { get; }
        public decimal Amount { get; }
        public string Tag { get; }
        public string Name { get; }
    }
}
