
namespace NetDream.Modules.CMS.Entities
{
    
    public class LinkageEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
