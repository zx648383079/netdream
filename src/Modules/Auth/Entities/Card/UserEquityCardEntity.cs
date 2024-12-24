
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Entities
{
    
    public class UserEquityCardEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public int Status { get; set; }
        
        public int ExpiredAt { get; set; }

        public int CardId { get; set; }
        public int Exp { get; set; }

        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        
 
        public EquityCardEntity? Card { get; set; }
    }
}
