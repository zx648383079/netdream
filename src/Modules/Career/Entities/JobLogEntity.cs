using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class JobLogEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int UserId { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}