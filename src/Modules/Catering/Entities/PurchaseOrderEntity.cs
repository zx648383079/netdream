using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class PurchaseOrderEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int UserId { get; set; }
    public byte Status { get; set; }
    public float Price { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int ExecuteId { get; set; }
    public int CheckId { get; set; }
    public int ExecuteAt { get; set; }
    public int CheckAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}