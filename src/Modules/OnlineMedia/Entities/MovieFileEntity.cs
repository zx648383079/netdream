using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class MovieFileEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public int SeriesId { get; set; }
    public byte FileType { get; set; }
    public byte Definition { get; set; }
    public string File { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    
}