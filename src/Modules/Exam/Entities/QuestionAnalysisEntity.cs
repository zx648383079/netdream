using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class QuestionAnalysisEntity : IIdEntity
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public byte Type { get; set; }
    public string Content { get; set; } = string.Empty;
    
}