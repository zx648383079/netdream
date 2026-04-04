using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Entities
{
    public class ShippingGroupEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int ShippingId { get; set; }
        
        public int IsAll { get; set; }
        
        public float FirstStep { get; set; }
        
        public float FirstFee { get; set; }
        public float Additional { get; set; }
        
        public float AdditionalFee { get; set; }
        
        public float FreeStep { get; set; }
    }
}
