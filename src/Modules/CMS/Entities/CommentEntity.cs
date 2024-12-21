
namespace NetDream.Modules.CMS.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public int Position { get; set; }
        
        public int UserId { get; set; }
        
        public int ModelId { get; set; }
        
        public int ContentId { get; set; }
        
        public int AgreeCount { get; set; }
        
        public int DisagreeCount { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
