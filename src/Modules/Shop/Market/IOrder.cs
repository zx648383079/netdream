namespace NetDream.Modules.Shop.Market
{
    public interface IOrder
    {

        public IOrderAddress Address { get; set; }
        public IOrderAddress Items { get; set; }
    }

    public interface IOrderProduct
    {

    }

    public interface IOrderAddress
    {

    }

    public interface IOrderCoupon
    {

    }

    public interface IOrderActivity
    {

    }
}
