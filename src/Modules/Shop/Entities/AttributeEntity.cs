
namespace NetDream.Modules.Shop.Entities
{
    
    public class AttributeEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int GroupId { get; set; }
        public int Type { get; set; }
        
        public int SearchType { get; set; }
        
        public int InputType { get; set; }
        
        public string DefaultValue { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
