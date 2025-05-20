using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class AwardEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public int GotAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}