
namespace NetDream.Modules.CMS.Entities
{
    
    public class GroupEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
