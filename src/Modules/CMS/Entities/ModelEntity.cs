
namespace NetDream.Modules.CMS.Entities
{
    
    public class ModelEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Table { get; set; } = string.Empty;
        public byte Type { get; set; }
        public byte Position { get; set; }
        
        public int ChildModel { get; set; }
        
        public string CategoryTemplate { get; set; } = string.Empty;
        
        public string ListTemplate { get; set; } = string.Empty;
        
        public string ShowTemplate { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
    }
}
