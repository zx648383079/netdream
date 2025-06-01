using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class BulletinUserEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        
        public int BulletinId { get; set; }

        public byte Status { get; set; }

        
        public int UserId { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

        public BulletinEntity? Bulletin { get; set; }

    }
}
