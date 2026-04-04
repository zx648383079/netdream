using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineService.Entities
{
    
    public class CategoryWordEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int CatId { get; set; }

        public CategoryEntity? Category { get; set; }
    }
}
