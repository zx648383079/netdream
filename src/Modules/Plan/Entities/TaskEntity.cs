using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Entities;
public class TaskEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int EveryTime { get; set; }
    public byte SpaceTime { get; set; }
    public int StartAt { get; set; }
    public byte PerTime { get; set; }
    public int TimeLength { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}