using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Team.Entities
{
    
    public class TeamEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int UserId { get; set; }

        public byte OpenType { get; set; }
        public string OpenRule { get; set; } = string.Empty;
        public byte Status { get; set; }

        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
