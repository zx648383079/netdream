using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class AttributeEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int GroupId { get; set; }
        public byte Type { get; set; }
        
        public byte SearchType { get; set; }
        
        public byte InputType { get; set; }
        
        public string DefaultValue { get; set; } = string.Empty;
        public int Position { get; set; }
        public string PropertyGroup { get; set; } = string.Empty;
    }
}
