using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class QuestionEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int MaterialId { get; set; }
    public int ParentId { get; set; }
    public byte Type { get; set; }
    public byte Easiness { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Dynamic { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int UserId { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    public int CourseGrade { get; set; }
    
}