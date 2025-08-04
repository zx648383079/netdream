
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineService.Entities
{
    
    public class CategoryUserEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int CatId { get; set; }
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public CategoryEntity? Category { get; set; }
    }
}
