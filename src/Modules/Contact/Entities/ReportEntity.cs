using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Contact.Entities
{
    
    public class ReportEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Files { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UserId { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
