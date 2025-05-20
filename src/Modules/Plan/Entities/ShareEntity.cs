using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Entities;
public class ShareEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public byte ShareType { get; set; }
    public string ShareRule { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}