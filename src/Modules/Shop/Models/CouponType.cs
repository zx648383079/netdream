namespace NetDream.Modules.Shop.Models
{
    public static class CouponType
    {
        public const byte TYPE_MONEY = 0;  // 优惠
        public const byte TYPE_DISCOUNT = 1; // 折扣

        public const byte RULE_NONE = 0;
        public const byte RULE_GOODS = 3;
        public const byte RULE_CATEGORY = 1;
        public const byte RULE_BRAND = 2;
        public const byte RULE_STORE = 4;

        public const byte SEND_RECEIVE = 0; // 前台领取
        public const byte SEND_GOODS = 1;   // 购买商品
        public const byte SEND_ORDER = 2;   // 订单金额
        public const byte SEND_SIGN = 3;    // 签到
        public const byte SEND_USER = 4;    // 按用户
    }
}
