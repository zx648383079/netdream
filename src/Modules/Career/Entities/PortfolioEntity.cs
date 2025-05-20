using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class PortfolioEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Trade { get; set; } = string.Empty;
    public string Function { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string Duty { get; set; } = string.Empty;
    public string Images { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}