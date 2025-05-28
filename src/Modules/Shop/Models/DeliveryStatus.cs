namespace NetDream.Modules.Shop.Models
{
    public static class DeliveryStatus
    {
        public const byte OrderPlaced = 0; // 已下单
        public const byte Processing = 1; // 出库中
        public const byte PickedUp = 2; // 已揽收
        public const byte InTransit = 3; // 运输中
        public const byte OutForDelivery = 4; // 派送中
        public const byte Delivered = 5; // 已签收
        public const byte Cancelled = 6; // 已取消
    }
}
