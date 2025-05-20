using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class SiteComponentEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public int SiteId { get; set; }
    public int CatId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public byte Type { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public byte Editable { get; set; }
    public string AliasName { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    public string Dependencies { get; set; } = string.Empty;
    
}