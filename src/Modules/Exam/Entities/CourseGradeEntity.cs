using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Entities;
public class CourseGradeEntity : IIdEntity
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; }
    
}