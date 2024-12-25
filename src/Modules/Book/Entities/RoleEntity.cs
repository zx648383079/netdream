
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Book.Entities
{
    
    public class RoleEntity: IIdEntity
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
    }
}
