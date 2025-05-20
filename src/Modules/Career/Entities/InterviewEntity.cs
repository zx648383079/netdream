using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class InterviewEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int CompanyId { get; set; }
    public int HrId { get; set; }
    public int UserId { get; set; }
    public string Address { get; set; } = string.Empty;
    public int InterviewAt { get; set; }
    public int EndAt { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}