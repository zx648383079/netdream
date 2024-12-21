
namespace NetDream.Modules.Counter.Entities
{
    
    public class LoadTimeLogEntity
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
