using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Contact.Entities
{
    
    public class FeedbackEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int OpenStatus { get; set; }
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
