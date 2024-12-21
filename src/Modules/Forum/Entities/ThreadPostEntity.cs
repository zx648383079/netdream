using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Entities
{
    
    public class ThreadPostEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int ThreadId { get; set; }
        
        public int UserId { get; set; }
        public string Ip { get; set; } = string.Empty;
        public int Grade { get; set; }
        
        public byte IsInvisible { get; set; }
        public byte Status { get; set; }
        
        public int AgreeCount { get; set; }
        
        public int DisagreeCount { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
