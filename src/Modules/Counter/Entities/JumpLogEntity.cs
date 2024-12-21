
namespace NetDream.Modules.Counter.Entities
{
    
    public class JumpLogEntity
    {
        
        public int Id { get; set; }
        public string Referrer { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;
        
        public string UserAgent { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
