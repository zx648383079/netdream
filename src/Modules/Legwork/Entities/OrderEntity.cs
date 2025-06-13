using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Entities;
public class OrderEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int UserId { get; set; }
    public int ServiceId { get; set; }
    public int Amount { get; set; }
    public string Remark { get; set; } = string.Empty;
    public decimal OrderAmount { get; set; }
    public int WaiterId { get; set; }
    public byte Status { get; set; }
    public byte ServiceScore { get; set; }
    public byte WaiterScore { get; set; }
    public int PayAt { get; set; }
    public int TakingAt { get; set; }
    public int TakenAt { get; set; }
    public int FinishAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}