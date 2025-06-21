using NetDream.Modules.Counter.Models;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class LogEntity : IIdEntity, ICreatedEntity, IUserAgent
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        
        public string BrowserVersion { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        
        public string OsVersion { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Referrer { get; set; } = string.Empty;
        
        public string UserAgent { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public string SessionId { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
