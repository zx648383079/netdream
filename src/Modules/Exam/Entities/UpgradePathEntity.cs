using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Exam.Entities;
public class UpgradePathEntity : IIdEntity
{
    public int Id { get; set; }
    public byte ItemType { get; set; }
    public int ItemId { get; set; }
    public int CourseGrade { get; set; }
    
}