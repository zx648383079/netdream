using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Entities
{
    
    public class ThreadLogEntity: IIdEntity, ICreatedEntity, IActionEntity
    {
        
        public int Id { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        public byte Action { get; set; }
        
        public byte NodeIndex { get; set; }
        public string Data { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
