using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class WorkExperienceEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int StartAt { get; set; }
    public int EndAt { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string Certificate { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}