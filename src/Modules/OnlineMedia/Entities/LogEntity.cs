using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class LogEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public byte ItemType { get; set; }
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public int Action { get; set; }
    public string Ip { get; set; } = string.Empty;
    public int CreatedAt { get; set; }
    
}