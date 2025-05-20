using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class PageQuestionEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public int EvaluateId { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    public int MaxScore { get; set; }
    public int Score { get; set; }
    public byte AnswerType { get; set; }
    public string Remark { get; set; } = string.Empty;
    
}