using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class PageEvaluateEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public int UserId { get; set; }
    public int SpentTime { get; set; }
    public int Right { get; set; }
    public int Wrong { get; set; }
    public int Score { get; set; }
    public byte Status { get; set; }
    public int MarkerId { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}