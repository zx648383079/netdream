using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserProfile.Entities
{
    
    public class BankCardEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Bank { get; set; } = string.Empty;
        public byte Type { get; set; }
        
        public string CardNo { get; set; } = string.Empty;
        
        public string ExpiryDate { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
