using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Repositories.Models;

namespace NetDream.Modules.Blog.Entities
{
    
    public class BlogEntity: IIdEntity, ILanguageEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public string ProgrammingLanguage { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        
        public int EditType { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int TermId { get; set; }
        public int Type { get; set; }
        
        public int RecommendCount { get; set; }
        
        public int CommentCount { get; set; }
        
        public int ClickCount { get; set; }
        
        public int OpenType { get; set; }
        
        public string OpenRule { get; set; } = string.Empty;
        
        public byte PublishStatus { get; set; }
        public byte Status { get; set; }

        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
