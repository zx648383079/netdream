using NetDream.Shared.Interfaces.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Entities
{
    
    public class BulletinUserEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        
        public int BulletinId { get; set; }

        public byte Status { get; set; }

        
        public int UserId { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

    }
}
