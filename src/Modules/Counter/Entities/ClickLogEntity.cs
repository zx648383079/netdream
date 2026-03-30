using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class ClickLogEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public int LogId { get; set; }
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        /// <summary>
        /// 点击的位置标签
        /// </summary>
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// 点击的标签链接
        /// </summary>
        public string TagUrl { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
