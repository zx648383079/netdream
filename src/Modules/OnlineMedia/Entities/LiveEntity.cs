using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class LiveEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}