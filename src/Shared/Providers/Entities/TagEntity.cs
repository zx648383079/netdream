using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    
    public class TagEntity : IIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
