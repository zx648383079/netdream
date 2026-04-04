using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Counter.Entities
{
    
    public class ClickLogEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public int LogId { get; set; }
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        /// <summary>
        /// �����λ�ñ�ǩ
        /// </summary>
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// ����ı�ǩ����
        /// </summary>
        public string TagUrl { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
