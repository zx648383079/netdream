using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class BulletinEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        
        public string ExtraRule { get; set; } = string.Empty;

        public byte Type { get; set; }

        
        public int UserId { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

        public ICollection<BulletinUserEntity>? Items { get; set; }

    }
}
