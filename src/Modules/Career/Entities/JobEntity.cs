using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class JobEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int UserId { get; set; }
    public byte Type { get; set; }
    public string Address { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public string Description { get; set; } = string.Empty;
    public float MinSalary { get; set; }
    public float MaxSalary { get; set; }
    public byte SalaryRule { get; set; }
    public byte SalaryType { get; set; }
    public string Longitude { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public byte Education { get; set; }
    public byte WorkTime { get; set; }
    public string Tags { get; set; } = string.Empty;
    public byte TopType { get; set; }
    public byte WeeklyDays { get; set; }
    public byte CheckPeriod { get; set; }
    public byte EmployPeriod { get; set; }
    public byte HeadCount { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}