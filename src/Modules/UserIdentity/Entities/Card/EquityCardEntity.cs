using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.UserIdentity.Entities
{
    
    public class EquityCardEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Configure { get; set; } = string.Empty;

        public byte Status { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

        public ICollection<UserEquityCardEntity>? Items { get; set; }

    }
}
