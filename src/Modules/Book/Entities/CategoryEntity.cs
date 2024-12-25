using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Entities
{
    
    public class CategoryEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }

        public ICollection<BookEntity>? Books { get; set; }

        public CategoryEntity()
        {
            
        }

        public CategoryEntity(string name)
        {
            Name = name;
        }
    }
}
