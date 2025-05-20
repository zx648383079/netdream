using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class SkillEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte Score { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}