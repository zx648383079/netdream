using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Finance.Entities;
public class FinancialProductEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte Status { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}