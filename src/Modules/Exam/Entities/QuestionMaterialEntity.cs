using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class QuestionMaterialEntity : IIdEntity
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public byte Type { get; set; }
    public string Content { get; set; } = string.Empty;
    
}