using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Entities
{
    
    public class MetaEntity : IIdEntity
    {
        public int Id { get; set; }
        public byte ItemType { get; set; }
        public int ItemId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
