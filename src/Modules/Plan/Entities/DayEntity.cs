using NetDream.Shared.Interfaces.Entities;
using System;

namespace NetDream.Modules.Plan.Entities;
public class DayEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public DateOnly Today { get; set; }
    public byte Amount { get; set; }
    public byte SuccessAmount { get; set; }
    public byte PauseAmount { get; set; }
    public byte FailureAmount { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}