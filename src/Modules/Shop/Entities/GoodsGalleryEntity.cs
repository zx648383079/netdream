using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsGalleryEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        public string File { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
