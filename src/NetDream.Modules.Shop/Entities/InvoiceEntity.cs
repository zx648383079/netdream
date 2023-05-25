using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class InvoiceEntity
    {
        internal const string ND_TABLE_NAME = "shop_invoice";
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
        public decimal Money { get; set; }
        public int Status { get; set; }
        [Column("invoice_type")]
        public int InvoiceType { get; set; }
        [Column("receiver_email")]
        public string ReceiverEmail { get; set; } = string.Empty;
        [Column("receiver_name")]
        public string ReceiverName { get; set; } = string.Empty;
        [Column("receiver_tel")]
        public string ReceiverTel { get; set; } = string.Empty;
        [Column("receiver_region_id")]
        public int ReceiverRegionId { get; set; }
        [Column("receiver_region_name")]
        public string ReceiverRegionName { get; set; } = string.Empty;
        [Column("receiver_address")]
        public string ReceiverAddress { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
