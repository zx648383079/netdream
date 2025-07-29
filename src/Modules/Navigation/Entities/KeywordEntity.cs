using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Navigation.Entities
{
    
    public class KeywordEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public byte Type { get; set; }
    }
}
