using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class PageLogEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public int HostId { get; set; }
        public int PathId { get; set; }
        
        public int VisitCount { get; set; }
  
    }
}
