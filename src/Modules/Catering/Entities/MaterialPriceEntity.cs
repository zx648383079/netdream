using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class MaterialPriceEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string MaterialId { get; set; } = string.Empty;
    public float Amount { get; set; }
    public byte Unit { get; set; }
    public float Price { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}