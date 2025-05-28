using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class OrderLabelItem
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public byte Status { get; set; }
        public int Count { get; set; }


        public OrderLabelItem(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public OrderLabelItem(byte status, int count)
            : this(status)
        {
            Count = count;
        }

        public OrderLabelItem(byte status)
        {
            Name = status switch
            {
                OrderStatus.STATUS_UN_PAY => "un_pay",
                OrderStatus.STATUS_PAYING => "paying",
                OrderStatus.STATUS_SHIPPED => "shipped",
                OrderStatus.STATUS_FINISH => "finish",
                OrderStatus.STATUS_CANCEL => "cancel",
                OrderStatus.STATUS_INVALID => "invalid",
                OrderStatus.STATUS_PAID_UN_SHIP => "paid_un_ship",
                OrderStatus.STATUS_RECEIVED => "received",
                OrderStatus.STATUS_REFUNDED => "refunded",
                _ => "unknown"
            };
            Label = Format(status);
            Status = status;
        }

        public static string Format(byte status)
        {
            return status switch
            {
                OrderStatus.STATUS_UN_PAY => "待支付",
                OrderStatus.STATUS_PAYING => "支付中",
                OrderStatus.STATUS_SHIPPED => "待收货",
                OrderStatus.STATUS_FINISH => "已完成",
                OrderStatus.STATUS_CANCEL => "已取消",
                OrderStatus.STATUS_INVALID => "已失效",
                OrderStatus.STATUS_PAID_UN_SHIP => "待发货",
                OrderStatus.STATUS_RECEIVED => "待评价",
                OrderStatus.STATUS_REFUNDED => "已退款",
                _ => "unknown"
            };
        }
    }
}
