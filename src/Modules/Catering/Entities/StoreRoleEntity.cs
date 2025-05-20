using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StoreRoleEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}