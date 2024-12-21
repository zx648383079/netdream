
namespace NetDream.Modules.Counter.Entities
{
    
    public class VisitorLogEntity
    {
        
        public int Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        
        public int FirstAt { get; set; }
        
        public int LastAt { get; set; }
    }
}
