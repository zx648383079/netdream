using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StoreStockEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int CatId { get; set; }
    public int MaterialId { get; set; }
    public float Amount { get; set; }
    public byte Unit { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}