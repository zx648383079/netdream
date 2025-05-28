namespace NetDream.Modules.Shop.Models
{
    public static class OrderStatus
    {
        public const byte STATUS_CANCEL = 1;
        public const byte STATUS_INVALID = 2;
        public const byte STATUS_UN_PAY = 10;
        public const byte STATUS_PAYING = 12;
        public const byte STATUS_PAID_UN_SHIP = 20;
        public const byte STATUS_SHIPPING = 38; // 发货中，部分发货
        public const byte STATUS_SHIPPED = 40;
        public const byte STATUS_RECEIVED = 60;
        public const byte STATUS_FINISH = 80;
        public const byte STATUS_REFUNDED = 81;

        public const byte TYPE_NONE = 0; //普通订单
        public const byte TYPE_AUCTION = 1; //拍卖订单
        public const byte TYPE_PRESELL = 2; //预售订单

        public const byte REFERENCE_USER = 1;
        public const byte REFERENCE_AD = 2;
    }
}
