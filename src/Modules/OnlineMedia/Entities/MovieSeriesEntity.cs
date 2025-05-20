using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class MovieSeriesEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int Episode { get; set; }
    public string Title { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}