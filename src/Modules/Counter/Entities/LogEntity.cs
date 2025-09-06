using NetDream.Modules.Counter.Models;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class LogEntity : IIdEntity, ICreatedEntity, IUserAgent
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;

        public string Hostname { get; set; } = string.Empty;
        public string Pathname { get; set; } = string.Empty;
        public string Queries { get; set; } = string.Empty;
        public string Method { get; set; } = "GET";
        public int StatusCode { get; set; } = 200;
        public string ReferrerHostname { get; set; } = string.Empty;
        public string ReferrerPathname { get; set; } = string.Empty;

        public string UserAgent { get; set; } = string.Empty;


        public int UserId { get; set; }
        
        public string SessionId { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
