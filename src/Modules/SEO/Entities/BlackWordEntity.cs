using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.SEO.Entities
{
    
    public class BlackWordEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Words { get; set; } = string.Empty;
        
        public string ReplaceWords { get; set; } = string.Empty;
    }
}
