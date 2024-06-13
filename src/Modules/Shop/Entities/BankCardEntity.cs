using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BankCardEntity
    {
        internal const string ND_TABLE_NAME = "shop_bank_card";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Bank { get; set; } = string.Empty;
        public int Type { get; set; }
        [Column("card_no")]
        public string CardNo { get; set; } = string.Empty;
        [Column("expiry_date")]
        public string ExpiryDate { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
