using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class OrderEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StoreId { get; set; }
    public int WaiterId { get; set; }
    public byte AddressType { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public string AddressTel { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int PaymentId { get; set; }
    public string PaymentName { get; set; } = string.Empty;
    public float GoodsAmount { get; set; }
    public float OrderAmount { get; set; }
    public float Discount { get; set; }
    public float ShippingFee { get; set; }
    public float PayFee { get; set; }
    public byte Status { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string ReserveAt { get; set; } = string.Empty;
    public int PayAt { get; set; }
    public int ShippingAt { get; set; }
    public int ReceiveAt { get; set; }
    public int FinishAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}