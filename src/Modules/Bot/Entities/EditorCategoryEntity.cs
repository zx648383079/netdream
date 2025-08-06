using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class EditorCategoryEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
    }
}
