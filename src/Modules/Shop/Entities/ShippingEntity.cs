
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class ShippingEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Method { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Position { get; set; }
        /// <summary>
        /// 是否支持货到付款
        /// </summary>

        public bool CodEnabled { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
