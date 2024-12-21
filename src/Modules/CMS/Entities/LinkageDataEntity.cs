
namespace NetDream.Modules.CMS.Entities
{
    
    public class LinkageDataEntity
    {
        
        public int Id { get; set; }
        
        public int LinkageId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public byte Position { get; set; }
        
        public string FullName { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
