using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Entities
{
    
    public class CommentEntity: IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public string ExtraRule { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public int Position { get; set; }
        
        public int UserId { get; set; }
        
        public int BlogId { get; set; }
        public string Ip { get; set; } = string.Empty;
        public string Agent { get; set; } = string.Empty;
        
        public int AgreeCount { get; set; }
        
        public int DisagreeCount { get; set; }
        public int Approved { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
