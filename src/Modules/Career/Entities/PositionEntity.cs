using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class PositionEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
    
}