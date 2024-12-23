using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.AdSense.Entities
{
    
    public class AdEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int PositionId { get; set; }
        public byte Type { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public int StartAt { get; set; }
        
        public int EndAt { get; set; }
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public PositionEntity? Position { get; set; }
    }
}
