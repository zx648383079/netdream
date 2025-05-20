using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class TagEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
}