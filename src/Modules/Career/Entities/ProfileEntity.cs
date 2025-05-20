using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class ProfileEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public int PositionId { get; set; }
    public byte Status { get; set; }
    public float Salary { get; set; }
    public byte SalaryRule { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}