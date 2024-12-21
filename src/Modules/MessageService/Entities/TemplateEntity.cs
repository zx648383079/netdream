
namespace NetDream.Modules.MessageService.Entities
{
    
    public class TemplateEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public string TargetNo { get; set; } = string.Empty;

        public byte Status { get; set; }

        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
