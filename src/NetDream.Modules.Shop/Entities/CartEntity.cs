using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CartEntity
    {
        internal const string ND_TABLE_NAME = "shop_cart";
        public int Id { get; set; }
        public int Type { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        [Column("is_checked")]
        public int IsChecked { get; set; }
        [Column("selected_activity")]
        public int SelectedActivity { get; set; }
        [Column("attribute_id")]
        public string AttributeId { get; set; } = string.Empty;
        [Column("attribute_value")]
        public string AttributeValue { get; set; } = string.Empty;
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
    }
}
