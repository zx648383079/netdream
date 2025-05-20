using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class SitePageEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public int ComponentId { get; set; }
    public byte Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Settings { get; set; } = string.Empty;
    public byte Position { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}