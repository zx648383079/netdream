using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class EducationalExperienceEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string School { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public byte Education { get; set; }
    public int StartAt { get; set; }
    public int EndAt { get; set; }
    public string Remark { get; set; } = string.Empty;
    public string Certificate { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}