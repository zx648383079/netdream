using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CouponEntity
    {
        internal const string ND_TABLE_NAME = "shop_coupon";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Rule { get; set; }
        [Column("rule_value")]
        public string RuleValue { get; set; } = string.Empty;
        [Column("min_money")]
        public decimal MinMoney { get; set; }
        public decimal Money { get; set; }
        [Column("send_type")]
        public int SendType { get; set; }
        [Column("send_value")]
        public int SendValue { get; set; }
        [Column("every_amount")]
        public int EveryAmount { get; set; }
        [Column("start_at")]
        public int StartAt { get; set; }
        [Column("end_at")]
        public int EndAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
