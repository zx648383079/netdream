using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ShippingGroupEntity
    {
        internal const string ND_TABLE_NAME = "shop_shipping_group";
        public int Id { get; set; }
        [Column("shipping_id")]
        public int ShippingId { get; set; }
        [Column("is_all")]
        public int IsAll { get; set; }
        [Column("first_step")]
        public float FirstStep { get; set; }
        [Column("first_fee")]
        public decimal FirstFee { get; set; }
        public float Additional { get; set; }
        [Column("additional_fee")]
        public decimal AdditionalFee { get; set; }
        [Column("free_step")]
        public float FreeStep { get; set; }
    }
}
