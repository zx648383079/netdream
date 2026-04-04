using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineMedia.Entities;
public class AreaEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
}