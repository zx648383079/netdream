
namespace NetDream.Modules.Shop.Entities
{
    
    public class ShippingGroupEntity
    {
        
        public int Id { get; set; }
        
        public int ShippingId { get; set; }
        
        public int IsAll { get; set; }
        
        public float FirstStep { get; set; }
        
        public decimal FirstFee { get; set; }
        public float Additional { get; set; }
        
        public decimal AdditionalFee { get; set; }
        
        public float FreeStep { get; set; }
    }
}
