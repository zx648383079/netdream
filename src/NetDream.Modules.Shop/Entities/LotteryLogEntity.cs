using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LotteryLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_lottery_log";
        public int Id { get; set; }
        [Column("act_id")]
        public int ActId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
