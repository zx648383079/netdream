using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserProfile.Entities
{
    
    public class CertificationEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Type { get; set; }
        
        public string CardNo { get; set; } = string.Empty;
        
        public string ExpiryDate { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public string FrontSide { get; set; } = string.Empty;
        
        public string BackSide { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
