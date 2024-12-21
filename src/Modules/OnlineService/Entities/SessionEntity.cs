using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineService.Entities
{
    
    public class SessionEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int ServiceId { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public string UserAgent { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int ServiceWord { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
