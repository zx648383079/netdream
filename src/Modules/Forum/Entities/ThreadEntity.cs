using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Entities
{
    
    public class ThreadEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int ForumId { get; set; }
        public int ZoneId { get; set; }

        public int ClassifyId { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int ViewCount { get; set; }
        
        public int PostCount { get; set; }
        
        public int CollectCount { get; set; }
        
        public byte IsHighlight { get; set; }
        
        public byte IsDigest { get; set; }
        
        public byte IsClosed { get; set; }
        
        public byte IsPrivatePost { get; set; }
        
        public byte TopType { get; set; }
        public byte Status { get; set; }

        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
