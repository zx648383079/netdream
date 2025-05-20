using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class PurchaseOrderGoodsEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MaterialId { get; set; }
    public float Amount { get; set; }
    public byte Unit { get; set; }
    public float Price { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}