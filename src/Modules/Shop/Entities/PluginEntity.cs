using NetDream.Shared.Interfaces;


namespace NetDream.Modules.Shop.Entities
{
    
    public class PluginEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Setting { get; set; } = string.Empty;

        public byte Status { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

    }
}
