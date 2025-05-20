using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class QuestionOptionEntity : IIdEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int QuestionId { get; set; }
    public byte Type { get; set; }
    public byte IsRight { get; set; }
    
}