using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Entities;
public class CommentEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public int LogId { get; set; }
    public string Content { get; set; } = string.Empty;
    public byte Type { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}