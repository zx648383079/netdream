
namespace NetDream.Modules.MicroBlog.Entities
{
    
    public class CommentEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public string ExtraRule { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public int UserId { get; set; }
        
        public int TargetId { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
        
        public int CreatedAt { get; set; }
        public uint AgreeCount { get; internal set; }
        public uint DisagreeCount { get; internal set; }
        public byte Status { get; internal set; }
    }
}
