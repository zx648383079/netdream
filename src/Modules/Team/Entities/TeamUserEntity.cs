using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Team.Entities
{
    
    public class TeamUserEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int TeamId { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int RoleId { get; set; }
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
