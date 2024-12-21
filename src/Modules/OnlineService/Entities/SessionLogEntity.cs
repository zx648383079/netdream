using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineService.Entities
{
    
    public class SessionLogEntity: IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int SessionId { get; set; }
        public string Remark { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
