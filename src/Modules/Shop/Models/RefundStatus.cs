namespace NetDream.Modules.Shop.Models
{
    public static class RefundStatus
    {
        public const byte STATUS_REFUSE = 0;     // 拒绝
        public const byte STATUS_IN_REVIEW = 10; // 审核中
        public const byte STATUS_DEALING = 20;   // 处理中
        public const byte STATUS_REFUNDING = 30;          // 退款中
        public const byte STATUS_FINISH = 40;          // 已完成
    }
}
