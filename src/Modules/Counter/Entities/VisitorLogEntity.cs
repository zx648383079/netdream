using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class VisitorLogEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int FirstAt { get; set; }
        
        public int LastAt { get; set; }
    }
}
