using NetDream.Modules.Counter.Models;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class ClickLogEntity : IIdEntity, ICreatedEntity, IUserAgent
    {
        
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;
        
        public string UserAgent { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        
        public string TagUrl { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
