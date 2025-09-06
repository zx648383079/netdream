using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class StayTimeLogEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public int LogId { get; set; }
        public byte Status { get; set; }
        
        public int EnterAt { get; set; }
        
        public int LeaveAt { get; set; }
    }
}
