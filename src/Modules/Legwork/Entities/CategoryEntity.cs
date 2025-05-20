using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Entities;
public class CategoryEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
}