
namespace NetDream.Modules.MicroBlog.Entities
{
    
    public class AttachmentEntity
    {
        
        public int Id { get; set; }
        
        public int MicroId { get; set; }
        public string Thumb { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
    }
}
