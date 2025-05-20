using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class ThemeStyleEntity : IIdEntity
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    
}