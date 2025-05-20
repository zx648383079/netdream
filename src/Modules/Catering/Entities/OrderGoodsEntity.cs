using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class OrderGoodsEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int GoodsId { get; set; }
    public int Amount { get; set; }
    public float Price { get; set; }
    public float Discount { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}