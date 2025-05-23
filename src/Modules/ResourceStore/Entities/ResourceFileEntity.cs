using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.ResourceStore.Entities;
public class ResourceFileEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ResId { get; set; }
    public byte FileType { get; set; }
    public string File { get; set; } = string.Empty;
    public int ClickCount { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}