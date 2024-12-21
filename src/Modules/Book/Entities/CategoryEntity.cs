
namespace NetDream.Modules.Book.Entities
{
    
    public class CategoryEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }

        public CategoryEntity()
        {
            
        }

        public CategoryEntity(string name)
        {
            Name = name;
        }
    }
}
