using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.AdSense.Entities
{
    
    public class PositionEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        
        public byte AutoSize { get; set; }
        
        public byte SourceType { get; set; }
        public string Width { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public ICollection<AdEntity> Items { get; set; }

        public PositionEntity()
        {
            
        }

        public PositionEntity(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
