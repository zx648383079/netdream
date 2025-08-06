using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class EditorTemplateEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public byte Type { get; set; }
        
        public int CatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
