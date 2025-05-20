using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StoreMetaEntity : IIdEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
}