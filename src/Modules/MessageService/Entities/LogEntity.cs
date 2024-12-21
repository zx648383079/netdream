using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.MessageService.Entities
{
    
    public class LogEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int TemplateId { get; set; }
        
        public byte TargetType { get; set; }
        public string Target { get; set; } = string.Empty;
        
        public string TemplateName { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
