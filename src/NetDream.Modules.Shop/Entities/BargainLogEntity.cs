using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BargainLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_bargain_log";
        public int Id { get; set; }
        [Column("bargain_id")]
        public int BargainId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
