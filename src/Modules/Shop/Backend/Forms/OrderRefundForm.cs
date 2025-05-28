namespace NetDream.Modules.Shop.Backend.Forms
{
    public class OrderRefundForm
    {

        public byte RefundType { get; set; }
        public float Money { get; set; }
    }

    public class OrderOperateForm
    {
        public string Remark { get; set; }
    }
    public class OrderShippingForm
    {
        public string LogisticsNumber { get; set; }
        public int ShippingId { get; set; }
    }
}
