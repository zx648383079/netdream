
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Book.Entities
{
    
    public class SourceEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        
        public int SiteId { get; set; }
        public string Url { get; set; } = string.Empty;
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public BookEntity? Book { get; set; }

        public SourceSiteEntity? Site { get; set; }
    }
}
