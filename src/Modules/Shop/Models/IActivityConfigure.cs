namespace NetDream.Modules.Shop.Models
{
    public interface IActivityConfigure
    {
    }

    public class AuctionConfigure : IActivityConfigure
    {
        public int Mode { get; set; }
        public float BeginPrice { get; set; }
        public float FixedPrice { get; set; }
        public float StepPrice { get; set; }
        public float Deposit { get; set; }
    }

    public class BargainConfigure : IActivityConfigure
    {
        public int Min {  get; set; }
        public int Max {  get; set; }
        public int Times {  get; set; }
        public int Amount {  get; set; }
        public int ShippingFee {  get; set; }
    }

    public class WholesaleConfigure : IActivityConfigure
    {
        public ActivityStepItem[] Items { get; set; }
    }

    public class CashBackConfigure : IActivityConfigure
    {
        public int OrderAmount { get; set; }
        public int Star { get; set; }
        public int Money { get; set; }
    }

    public class DiscountConfigure : IActivityConfigure
    {
        public int Type { get; set; }
        public int Amount { get; set; }
        public int CheckDiscount { get; set; }
        public int CheckMoney { get; set; }
        public int CheckGift { get; set; }
        public int CheckShipping { get; set; }
        public int DiscountValue { get; set; }
        public int DiscountMoney { get; set; }
        public int DiscountGoods { get; set; }
    }

    public class FreeTrialConfigure : IActivityConfigure
    {
        public int Amount { get; set; }
    }

    public class GroupBuyConfigure : IActivityConfigure
    {
        public int Deposit { get; set; }
        public int Amount { get; set; }
        public int MinUsers { get; set; }
        public int MaxUsers { get; set; }
        public int SendPoint { get; set; }
        public ActivityStepItem[] Step { get; set; }
    }

    public class ActivityStepItem
    {
        public int Amount { get; set; }
        public float Price { get; set; }
    }

    public class LotteryConfigure : IActivityConfigure
    {
        public float TimePrice { get; set; }
        public int BuyTimes { get; set; }
        public int StartTimes { get; set; }
        public string BtnText { get; set; }
        public string OverText { get; set; }
        public LotteryGiftItem[] Items { get; set; }

        public class LotteryGiftItem
        {
            public string name { get; set; }
            public int GoodsId { get; set; }
            public int Chance { get; set; }
            public string Color { get; set; }
        }
    }

    public class MixConfigure : IActivityConfigure
    {
        public float Price { get; set; }
        public MixGoodsItem[] Goods { get; set; }

        public class MixGoodsItem
        {
            public int GoodsId { get; set; }
            public int Amount { get; set; }
            public int Price { get; set; }
        }
    }

    public class PreSaleConfigure : IActivityConfigure
    {
        public string FinalStartAt { get; set; }
        public string FinalEndAt { get; set; }
        public string ShipAt { get; set; }
        public int PriceType { get; set; }
        public float Price { get; set; }
        public float Step { get; set; }
        public float Deposit { get; set; }
        public float DepositScale { get; set; }
        public float DepositScaleOther { get; set; }
    }
}
