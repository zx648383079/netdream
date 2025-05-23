using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.ResourceStore.Entities;
public class ResourceMetaEntity : IIdEntity
{
    public int Id { get; set; }
    public int ResId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
}