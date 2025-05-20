using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Entities;
public class ProviderEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public byte OverallRating { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}