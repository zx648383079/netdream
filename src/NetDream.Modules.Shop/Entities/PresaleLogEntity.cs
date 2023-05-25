using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PresaleLogEntity
    {
        internal const string ND_TABLE_NAME = "shop_presale_log";
        public int Id { get; set; }
        [Column("act_id")]
        public int ActId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("order_goods_id")]
        public int OrderGoodsId { get; set; }
        [Column("order_amount")]
        public decimal OrderAmount { get; set; }
        public decimal Deposit { get; set; }
        [Column("final_payment")]
        public decimal FinalPayment { get; set; }
        public int Status { get; set; }
        [Column("predetermined_at")]
        public int PredeterminedAt { get; set; }
        [Column("final_at")]
        public int FinalAt { get; set; }
        [Column("ship_at")]
        public int ShipAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
