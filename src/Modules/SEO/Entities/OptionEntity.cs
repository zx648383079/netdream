
namespace NetDream.Modules.SEO.Entities
{
    
    public class OptionEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Visibility { get; set; }
        
        public string DefaultValue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
