using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Exam.Entities;
public class QuestionWrongEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public int Frequency { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}