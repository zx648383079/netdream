using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.OnlineService.Entities
{
    
    public class CategoryEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<CategoryWordEntity>? Words { get; set; }
        public ICollection<CategoryUserEntity>? Items { get; set; }
    }
}
