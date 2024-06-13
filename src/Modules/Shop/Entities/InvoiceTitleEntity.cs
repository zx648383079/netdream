using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class InvoiceTitleEntity
    {
        internal const string ND_TABLE_NAME = "shop_invoice_title";
        public int Id { get; set; }
        [Column("title_type")]
        public int TitleType { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("tax_no")]
        public string TaxNo { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
