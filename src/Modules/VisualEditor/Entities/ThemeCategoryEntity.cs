using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class ThemeCategoryEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    
}