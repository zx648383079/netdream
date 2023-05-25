using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OrderLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_order_log";
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
