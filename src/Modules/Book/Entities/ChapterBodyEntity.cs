
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Book.Entities
{
    
    public class ChapterBodyEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
