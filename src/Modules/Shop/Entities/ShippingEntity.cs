
using NetDream.Shared.Interfaces;

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
        /// �Ƿ�֧�ֻ�������
        /// </summary>

        public bool CodEnabled { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
