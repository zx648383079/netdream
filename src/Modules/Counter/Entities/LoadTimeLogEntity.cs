using NetDream.Modules.Counter.Models;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class LoadTimeLogEntity : IIdEntity, ICreatedEntity, IUserAgent
    {
        
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;
        
        public string UserAgent { get; set; } = string.Empty;
        
        public int LoadTime { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
