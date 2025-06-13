
namespace NetDream.Modules.Shop.Backend.Forms
{
    public class CouponForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public byte Type { get; set; }
        public byte Rule { get; set; }

        public string RuleValue { get; set; } = string.Empty;

        public float MinMoney { get; set; }
        public float Money { get; set; }

        public int SendType { get; set; }

        public int SendValue { get; set; }

        public int EveryAmount { get; set; }

        public int StartAt { get; set; }

        public int EndAt { get; set; }
    }
}
