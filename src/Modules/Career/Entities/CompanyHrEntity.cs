using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class CompanyHrEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public string Description { get; set; } = string.Empty;
    public byte Credit { get; set; }
    public string Longitude { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}