using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Entities
{
    
    public class InviteCodeEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public int Amount { get; set; }

        public byte Type { get; set; }
        public string Token { get; set; } = string.Empty;

        public int InviteCount { get; set; }
        
        public int ExpiredAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
     
    }
}
