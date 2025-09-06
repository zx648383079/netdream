using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    
    public class LoadTimeLogEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public int LogId { get; set; }

        public int LoadTime { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
