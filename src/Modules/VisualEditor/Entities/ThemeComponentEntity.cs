using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class ThemeComponentEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public int CatId { get; set; }
    public int UserId { get; set; }
    public int Price { get; set; }
    public byte Type { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public byte Status { get; set; }
    public byte Editable { get; set; }
    public string Path { get; set; } = string.Empty;
    public string AliasName { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    public string Dependencies { get; set; } = string.Empty;
    
}