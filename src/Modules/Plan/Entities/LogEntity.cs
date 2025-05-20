using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Entities;
public class LogEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public int ChildId { get; set; }
    public int DayId { get; set; }
    public byte Status { get; set; }
    public int OutageTime { get; set; }
    public int EndAt { get; set; }
    public int CreatedAt { get; set; }
    
}