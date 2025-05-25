using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class CourseListItem : CourseEntity
    {
        public CourseEntity[] Children { get; set; }
    }
}
