using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class MusicFileEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public int MusicId { get; set; }
    public byte FileType { get; set; }
    public string File { get; set; } = string.Empty;
    public int CreatedAt { get; set; }
    
}