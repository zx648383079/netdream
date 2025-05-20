using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class MovieScoreEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Score { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Url { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}