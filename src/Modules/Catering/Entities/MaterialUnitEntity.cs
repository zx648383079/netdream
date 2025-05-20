using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class MaterialUnitEntity : IIdEntity
{
    public int Id { get; set; }
    public string MaterialId { get; set; } = string.Empty;
    public float FromAmount { get; set; }
    public byte FromUnit { get; set; }
    public float ToAmount { get; set; }
    public byte ToUnit { get; set; }
    
}