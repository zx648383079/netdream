using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.ResourceStore.Entities;
public class CategoryEntity : IIdEntity, ITreeLinkItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ParentId { get; set; }
    public string Keywords { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public byte IsHot { get; set; }
    
}