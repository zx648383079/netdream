using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class PageEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte RuleType { get; set; }
    public string RuleValue { get; set; } = string.Empty;
    public int StartAt { get; set; }
    public int EndAt { get; set; }
    public int LimitTime { get; set; }
    public int UserId { get; set; }
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    public int Score { get; set; }
    public int QuestionCount { get; set; }
    public int CourseId { get; set; }
    public int CourseGrade { get; set; }
    
}