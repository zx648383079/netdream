using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AuctionLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_auction_log";
        public int Id { get; set; }
        [Column("act_id")]
        public int ActId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public decimal Bid { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
