
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Entities
{
    
    public class ListEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int BookCount { get; set; }
        
        public int ClickCount { get; set; }
        
        public int CollectCount { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public ICollection<ListItemEntity>? Items { get; set; }
    }
}
