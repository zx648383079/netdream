using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Finance.Entities;
public class BudgetEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Budget { get; set; }
    public float Spent { get; set; }
    public byte Cycle { get; set; }
    public int UserId { get; set; }
    public int DeletedAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}