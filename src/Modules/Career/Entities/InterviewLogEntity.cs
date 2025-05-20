using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class InterviewLogEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int InterviewId { get; set; }
    public int UserId { get; set; }
    public byte Type { get; set; }
    public string Data { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}