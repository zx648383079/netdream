namespace NetDream.Modules.Shop.Market.Models
{
    public class OrderSubtotal
    {

        public int UnPay { get; set; }
        public int Shipped { get; set; }
        public int Finish { get; set; }
        public int Cancel { get; set; }
        public int Invalid { get; set; }
        public int PaidUnShip { get; set; }
        public int Received { get; set; }
        public int Uncomment { get; set; }
        public int Refunding { get; set; }
    }
}
