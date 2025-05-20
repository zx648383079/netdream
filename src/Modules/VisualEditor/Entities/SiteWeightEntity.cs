using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class SiteWeightEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public int ComponentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Settings { get; set; } = string.Empty;
    public int StyleId { get; set; }
    public byte IsShare { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}