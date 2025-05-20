using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class CompanyEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int IndustryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public string Description { get; set; } = string.Empty;
    public byte Credit { get; set; }
    public byte Type { get; set; }
    public byte EmployeeCount { get; set; }
    public byte FinancingStage { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}