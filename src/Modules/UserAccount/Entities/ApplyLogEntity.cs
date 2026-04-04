using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class ApplyLogEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public byte Type { get; set; }
        public byte ItemType { get; set; }
        public int ItemId { get; set; }
        public int Money { get; set; }
        public string Remark { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
