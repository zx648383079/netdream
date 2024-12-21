
namespace NetDream.Modules.CMS.Entities
{
    
    public class ModelFieldEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        
        public int ModelId { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Length { get; set; }
        public byte Position { get; set; }
        
        public byte FormType { get; set; }
        
        public byte IsMain { get; set; }
        
        public byte IsRequired { get; set; }
        
        public byte IsSearch { get; set; }
        
        public byte IsDisable { get; set; }
        
        public byte IsSystem { get; set; }
        public string Match { get; set; } = string.Empty;
        
        public string TipMessage { get; set; } = string.Empty;
        
        public string ErrorMessage { get; set; } = string.Empty;
        
        public string TabName { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
    }
}
