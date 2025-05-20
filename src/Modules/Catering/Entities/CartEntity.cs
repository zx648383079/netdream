using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class CartEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public byte Type { get; set; }
    public int UserId { get; set; }
    public int StoreId { get; set; }
    public int GoodsId { get; set; }
    public int Amount { get; set; }
    public float Price { get; set; }
    public float Discount { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}